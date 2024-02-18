using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
        NetworkManager.Singleton.StartClient();
    }

    // Checks if the players can set themselves as ready when in character select scene.
    public void CheckPlayersCanSetReadyCharacterSelect()
    {
        CheckPlayersCanSetReadyCharacterSelectServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void CheckPlayersCanSetReadyCharacterSelectServerRpc()
    {
        string errorMessage = PlayerCanSetReadyCharacterSelect();
        bool canSetReady = errorMessage == "";

        if (!canSetReady)
        {
            _playerReadyCharacterSelect.Clear();
        }
        
        NotifyPlayerSetReadyCharacterSelectClientRpc(canSetReady, errorMessage);
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

    public void SetPlayerReadyCharacterSelect(bool isReady)
    {
        SetPlayerReadyCharacterSelectServerRpc(isReady);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyCharacterSelectServerRpc(bool isReady, ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        
        _playerReadyCharacterSelect[clientId] = isReady;

        if (AreAllPlayersReady())
        {
            // TODO transition vers scene de jeu.
            Debug.Log("BOTH PLAYER ARE READY :)");
        }
    }

    private bool AreAllPlayersReady()
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
        _playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId,
            characterSelection = CharacterSelectUI.CharacterId.First
        });
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
