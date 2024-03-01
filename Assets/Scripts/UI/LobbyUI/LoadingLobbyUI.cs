using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class LoadingLobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    
    private const string JOINING_LOBBY_TEXT = "JOINING LOBBY...";
    private const string CREATING_LOBBY_TEXT = "CREATING LOBBY...";
    
    private void Start()
    {
        GameLobbyManager.Instance.OnJoinStarted += GameLobbyManager_OnJoinStarted;
        GameLobbyManager.Instance.OnJoinFailed += GameLobbyManager_OnJoinFailed;
        
        GameLobbyManager.Instance.OnCreateLobbyStarted += GameLobbyManager_OnCreateLobbyStarted;
        GameLobbyManager.Instance.OnCreateLobbyFailed += GameLobbyManager_OnCreateLobbyFailed;
        
        BasicShowHide.Hide(gameObject);
    }

    private void GameLobbyManager_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        loadingText.text = CREATING_LOBBY_TEXT;
        BasicShowHide.Show(gameObject);
    }

    private void GameLobbyManager_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        BasicShowHide.Hide(gameObject);
    }
    
    private void GameLobbyManager_OnJoinStarted(object sender, EventArgs e)
    {
        loadingText.text = JOINING_LOBBY_TEXT;
        BasicShowHide.Show(gameObject);
    }
    
    private void GameLobbyManager_OnJoinFailed(object sender, EventArgs e)
    {
        BasicShowHide.Hide(gameObject);
    }


    private void OnDestroy()
    {
        GameLobbyManager.Instance.OnJoinStarted -= GameLobbyManager_OnJoinStarted;
        GameLobbyManager.Instance.OnJoinFailed -= GameLobbyManager_OnJoinFailed;
        
        GameLobbyManager.Instance.OnCreateLobbyStarted -= GameLobbyManager_OnCreateLobbyStarted;
        GameLobbyManager.Instance.OnCreateLobbyFailed -= GameLobbyManager_OnCreateLobbyFailed;
    }
}
