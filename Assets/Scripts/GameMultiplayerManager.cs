using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class GameMultiplayerManager : NetworkBehaviour
{
    public static GameMultiplayerManager Instance { get; private set; }
    
    public static int MAX_NUMBER_OF_PLAYERS = 2;

    private NetworkList<PlayerData> _playerDataNetworkList;

    private void Awake()
    {
        Instance = this;

        _playerDataNetworkList = new NetworkList<PlayerData>();
        _playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;

        _playerReadyCharacterSelect = new Dictionary<ulong, bool>();
        
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Host_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Host_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
    }

    // Checks if the players can set themselves as ready when in character select scene.
    public void CheckPlayersCanSetReadyCharacterSelect()
    {
        CheckPlayersCanSetReadyCharacterSelectServerRpc();
    }

    
    // Lance une exception NoClientException lorsqu'aucun client est présent.
    public PlayerData GetClientPlayerData()
    {
        PlayerData playerDataOfClient = default;

        if (_playerDataNetworkList.Count == MAX_NUMBER_OF_PLAYERS)
        {
            foreach (PlayerData playerData in _playerDataNetworkList)
            {
                if (playerData.clientId != NetworkManager.ServerClientId)
                {
                    playerDataOfClient = playerData;
                }
            }
        }
        else
        {
            throw new NoClientException("No client connected !");
        }

        return playerDataOfClient;
    }

    private const string KICK_REASON = "YOU HAVE BEEN KICKED";
        
    public void KickPlayer(ulong clientId)
    {
        if (IsServer)
        {
            NetworkManager.Singleton.DisconnectClient(clientId, KICK_REASON);

            TryRemoveDisconnectedPlayerFromPlayerReadyList(clientId);
        }
    }

    private void TryRemoveDisconnectedPlayerFromPlayerReadyList(ulong disconnectedClientId)
    {
        if (_playerReadyCharacterSelect.ContainsKey(disconnectedClientId))
        {
            _playerReadyCharacterSelect.Remove(disconnectedClientId);
            
            ResetPlayerReady();
        }
    }

    public event EventHandler OnPlayerReadyReset;
    
    [ServerRpc(RequireOwnership = false)]
    private void CheckPlayersCanSetReadyCharacterSelectServerRpc()
    {
        string errorMessage = PlayerCanSetReadyCharacterSelect();
        bool canSetReady = errorMessage == "";

        if (!canSetReady)
        {
            ResetPlayerReady();
        }
        
        NotifyPlayerSetReadyCharacterSelectClientRpc(canSetReady, errorMessage);
    }

    private void ResetPlayerReady()
    {
        ResetPlayerReadyClientRpc();
        
        _playerReadyCharacterSelect.Clear();
    }

    [ClientRpc]
    private void ResetPlayerReadyClientRpc()
    {
        OnPlayerReadyReset?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<OnPlayerReadyCharacterSelectCheckedEventArgs> OnPlayerReadyCharacterSelectChecked;

    public class OnPlayerReadyCharacterSelectCheckedEventArgs : EventArgs
    {
        public bool canSetReady;
        public string errorMessage;
    }
    
    [ClientRpc]
    private void NotifyPlayerSetReadyCharacterSelectClientRpc(bool canSetReady, string errorMessage)
    {
        OnPlayerReadyCharacterSelectChecked?.Invoke(this, new OnPlayerReadyCharacterSelectCheckedEventArgs
        {
            canSetReady = canSetReady,
            errorMessage = errorMessage
        });
    }
    
    /**
     * En cas d'erreur, retourne le message d'erreur ne permettant pas aux joueurs de se mettre prêt.
     * Sinon, retourne un message vide.
     */
    private string PlayerCanSetReadyCharacterSelect()
    {
        if (AnyPlayerIsMissing())
        {
            return PlayerReadyCharacterSelectionUI.MISSING_PLAYER_ERROR;
        }
        
        if (PlayersSelectedSameCharacter())
        {
            return PlayerReadyCharacterSelectionUI.SAME_CHARACTER_SELECTION_ERROR;
        }

        if (AnyPlayerHasNotSelectCharacter())
        {
            return PlayerReadyCharacterSelectionUI.HAVE_TO_SELECT_CHARACTER_ERROR;
        }

        return "";
    }

    private bool AnyPlayerIsMissing()
    {
        return NetworkManager.Singleton.ConnectedClients.Count != MAX_NUMBER_OF_PLAYERS;
    }

    private bool PlayersSelectedSameCharacter()
    {
        (CharacterSelectUI.CharacterId, CharacterSelectUI.CharacterId) playersSelection =
            GameMultiplayerManager.Instance.GetCharacterVisualSelection();

        return playersSelection.Item1 == playersSelection.Item2;
    }
    
    private bool AnyPlayerHasNotSelectCharacter()
    {
        (CharacterSelectUI.CharacterId, CharacterSelectUI.CharacterId)
            playersSelection = GameMultiplayerManager.Instance.GetCharacterVisualSelection();
        
        return playersSelection.Item1 == CharacterSelectUI.CharacterId.None ||
               playersSelection.Item2 == CharacterSelectUI.CharacterId.None;
    }

    
    // clientId et si pret ou non
    private Dictionary<ulong, bool> _playerReadyCharacterSelect;
    
    public event EventHandler<OnPlayerReadyCharacterSelectChangedEventArgs> OnPlayerReadyCharacterSelectChanged;
    public class OnPlayerReadyCharacterSelectChangedEventArgs : EventArgs
    {
        public bool isHost;
        public bool isReady;
    }
    
    public void SetPlayerReadyCharacterSelect(bool isReady)
    {
        SetPlayerReadyCharacterSelectServerRpc(isReady, IsHost);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyCharacterSelectServerRpc
        (bool isReady, bool isHost, ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyCharacterSelectClientRpc(isReady, isHost);
        
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        
        _playerReadyCharacterSelect[clientId] = isReady;

        if (AreAllPlayersReadyCharacterSelect())
        {
            // TODO transition vers scene de jeu
            Debug.Log("BOTH PLAYER ARE READY :)");
            Loader.LoadNetwork(Loader.Scene.Blocks);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyCharacterSelectClientRpc
        (bool isReady, bool isHost) 
    {
        OnPlayerReadyCharacterSelectChanged?.Invoke(this, new OnPlayerReadyCharacterSelectChangedEventArgs
        {
            isHost = isHost,
            isReady = isReady
        });
    }

    private bool AreAllPlayersReadyCharacterSelect()
    {
        bool areAllReady = true;
        
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            areAllReady = _playerReadyCharacterSelect.ContainsKey(clientId) && _playerReadyCharacterSelect[clientId];

            if (! areAllReady) { break; }
        }

        return areAllReady;
    }

    public void SelectCharacterVisual(CharacterSelectUI.CharacterId selectedId)
    {
        SelectCharacterVisualServerRpc(selectedId);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SelectCharacterVisualServerRpc
        (CharacterSelectUI.CharacterId selectedId, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        
        PlayerData toUpdate = _playerDataNetworkList[playerDataIndex];

        toUpdate.characterSelection = selectedId;

        _playerDataNetworkList[playerDataIndex] = toUpdate;
    }
    
    /**
     * Returns host, as Item1, and client, as Item2, character selection.
     */
    public (CharacterSelectUI.CharacterId, CharacterSelectUI.CharacterId) GetCharacterVisualSelection()
    {
        if (_playerDataNetworkList.Count == MAX_NUMBER_OF_PLAYERS)
        {
            return (_playerDataNetworkList[0].characterSelection, _playerDataNetworkList[1].characterSelection);
        }
        else
        {
            return (_playerDataNetworkList[0].characterSelection, CharacterSelectUI.CharacterId.None);
        }
    }

    private const string NO_CLIENT_ID_MATCH_CHARACTER_SELECTION =
        "No matching client id found when searching for character selection";
    
    /**
     * Throws NoMatchingClientIdFoundException.
     */
    public CharacterSelectUI.CharacterId GetCharacterSelectionFromClientId(ulong clientId)
    {
        foreach (PlayerData playerData in _playerDataNetworkList)
        {
            if (playerData.clientId == clientId)
            {
                return playerData.characterSelection;
            }
        }

        throw new NoMatchingClientIdFoundException(NO_CLIENT_ID_MATCH_CHARACTER_SELECTION);
    }

    /**
     * Returns -1 if no equivalence found.
     */
    private int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        int equivalentIndex = -1;

        for (int i = 0; i < _playerDataNetworkList.Count; i++)
        {
            PlayerData toCompare = _playerDataNetworkList[i];
            
            if (toCompare.clientId == clientId)
            {
                equivalentIndex = i;
            }
        }

        return equivalentIndex;
    }
    
    public event EventHandler OnPlayerDataNetworkListChanged;
    
    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }
    
    private void NetworkManager_Host_OnClientConnectedCallback(ulong clientId)
    {
        CharacterSelectUI.CharacterId initial = clientId == NetworkManager.ServerClientId ? 
            CharacterSelectUI.CharacterId.Monkey : CharacterSelectUI.CharacterId.Robot;
        
            _playerDataNetworkList.Add(new PlayerData
            {
                clientId = clientId,
                characterSelection = initial,
            });
        
        SetLobbyPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }
    
    private void NetworkManager_Client_OnClientConnectedCallback(ulong clientId)
    {
        SetLobbyPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    public event EventHandler OnHostDisconneted;
    
    private void NetworkManager_Client_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
        {
            OnHostDisconneted?.Invoke(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetLobbyPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData toSetPlayerId = _playerDataNetworkList[playerDataIndex];

        toSetPlayerId.lobbyPlayerId = playerId;
        
        _playerDataNetworkList[playerDataIndex] = toSetPlayerId;
    }

    private void NetworkManager_Host_OnClientDisconnectCallback(ulong clientId)
    {
        foreach (PlayerData playerData in _playerDataNetworkList)
        {
            if (playerData.clientId == clientId)
            {
                _playerDataNetworkList.Remove(playerData);
            }
        }
    }
}
