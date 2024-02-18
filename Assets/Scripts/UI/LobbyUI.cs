using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinLobbyButton;

    [SerializeField] private CreateLobbyUI createLobbyUI;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        
        createLobbyButton.onClick.AddListener(() =>
        {
            createLobbyUI.Show();
        });
        
        quickJoinLobbyButton.onClick.AddListener(() =>
        {
            GameLobbyManager.Instance.QuickJoin();
        });
    }
}
