using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ErrorLobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private Button closeErrorLobbyButton;

    private void Awake()
    {
        closeErrorLobbyButton.onClick.AddListener(() =>
        {
            BasicShowHide.Hide(gameObject);
        });
    }

    private void Start()
    {
        GameLobbyManager.Instance.OnJoinFailed += GameLobbyManager_OnJoinFailed;
        GameLobbyManager.Instance.OnQuickJoinFailed += GameLobbyManager_OnQuickJoinFailed;
        
        GameLobbyManager.Instance.OnCreateLobbyFailed += GameLobbyManager_OnCreateLobbyFailed;
        
        GameMultiplayerManager.Instance.OnFailedToJoinGame += GameMultiplayerManager_OnFailedToJoinGame;
        
        BasicShowHide.Hide(gameObject);
    }

    private const string ON_JOIN_FAILED_ERROR_TEXT = "Failed to join a lobby !";
    private void GameLobbyManager_OnJoinFailed(object sender, EventArgs e)
    {
        ShowWithErrorText(ON_JOIN_FAILED_ERROR_TEXT);
    }

    private const string ON_QUICK_JOIN_FAILED_TEXT = "Could not find a lobby to join !";
    private void GameLobbyManager_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowWithErrorText(ON_QUICK_JOIN_FAILED_TEXT);
    }

    private const string ON_CREATE_LOBBY_FAILED_TEXT = "Failed to create a lobby !";
    private void GameLobbyManager_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowWithErrorText(ON_CREATE_LOBBY_FAILED_TEXT);
    }

    private const string DEFAULT_ON_FAILED_TO_JOIN_TEXT = "Failed to join game !";
    private void GameMultiplayerManager_OnFailedToJoinGame(object sender, EventArgs e)
    {
        string errorToShow = NetworkManager.Singleton.DisconnectReason;
        
        if (errorToShow == "")
        {
            errorToShow = DEFAULT_ON_FAILED_TO_JOIN_TEXT;
        }
        
        ShowWithErrorText(errorToShow);
    }

    private void ShowWithErrorText(string errorToShow)
    {
        errorText.text = errorToShow;
        BasicShowHide.Show(gameObject);
    }
    
    private void OnDestroy()
    {
        GameLobbyManager.Instance.OnJoinFailed -= GameLobbyManager_OnJoinFailed;
        GameLobbyManager.Instance.OnQuickJoinFailed -= GameLobbyManager_OnQuickJoinFailed;
        
        GameLobbyManager.Instance.OnCreateLobbyFailed -= GameLobbyManager_OnCreateLobbyFailed;
        
        GameMultiplayerManager.Instance.OnFailedToJoinGame -= GameMultiplayerManager_OnFailedToJoinGame;
    }
}
