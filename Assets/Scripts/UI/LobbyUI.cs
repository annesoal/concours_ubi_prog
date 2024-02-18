using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinLobbyButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            // Loader.load()
        });
    }
}
