using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyListUI : MonoBehaviour
{
    [SerializeField] private Transform singleLobbyListTemplate;
    
    private void Start()
    {
        GameLobbyManager.Instance.OnLobbyListChanged += GameLobbyManager_OnLobbyListChanged;
    }

    private void GameLobbyManager_OnLobbyListChanged(object sender, GameLobbyManager.OnLobbyListChangedEventArgs e)
    {
        CleanUI();
        
        UpdateUI(e.lobbyList);
    }

    private void CleanUI()
    {
        foreach (Transform child in transform)
        {
            if (child != singleLobbyListTemplate)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void UpdateUI(List<Lobby> lobbiesToShow)
    {
        foreach (Lobby lobby in lobbiesToShow)
        {
            Transform toUpdate = Instantiate(singleLobbyListTemplate, transform);
            
            toUpdate.gameObject.SetActive(true);
            
            toUpdate.GetComponent<SingleLobbyListUI>().UpdateVisuals(lobby);
        }
    }

    private void OnDestroy()
    {
        GameLobbyManager.Instance.OnLobbyListChanged -= GameLobbyManager_OnLobbyListChanged;
    }
}
