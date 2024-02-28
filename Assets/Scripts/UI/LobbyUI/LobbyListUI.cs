using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyListUI : MonoBehaviour
{
    private void Start()
    {
        GameLobbyManager.Instance.OnLobbyListChanged += GameLobbyManager_OnLobbyListChanged;
    }

    private void GameLobbyManager_OnLobbyListChanged(object sender, GameLobbyManager.OnLobbyListChangedEventArgs e)
    {
        // TODO SHOW RESULT
        Debug.Log("UPDATED LOBBY LIST");
    }

    private void OnDestroy()
    {
        GameLobbyManager.Instance.OnLobbyListChanged -= GameLobbyManager_OnLobbyListChanged;
    }
}
