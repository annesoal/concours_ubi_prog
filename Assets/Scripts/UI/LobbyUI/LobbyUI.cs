using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinLobbyButton;
    [SerializeField] private Button joinWithCodeButton;
    [SerializeField] private TMP_InputField lobbyCodeInputField;

    [SerializeField] private CreateLobbyUI createLobbyUI;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(Loader.ReturnToMainMenuClean);
        
        createLobbyButton.onClick.AddListener(() =>
        {
            createLobbyUI.Show();
        });
        
        quickJoinLobbyButton.onClick.AddListener(() =>
        {
            GameLobbyManager.Instance.QuickJoin();
        });
        
        joinWithCodeButton.onClick.AddListener(() =>
        {
            GameLobbyManager.Instance.JoinLobbyByCode(lobbyCodeInputField.text.Trim());
        });
    }
}
