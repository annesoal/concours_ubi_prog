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

    public void SelectCharacterVisual(CharacterSelectUI.CharacterId characterId)
    {
        SelectCharacterServerRpc(characterId);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SelectCharacterServerRpc
        (CharacterSelectUI.CharacterId characterId, ServerRpcParams serverRpcParams = default)
    {
        // change _playerDataNetworkList for characterVisualId
    }
    
    /**
     * Returns host, as Item1, and client, as Item2, character selection
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
