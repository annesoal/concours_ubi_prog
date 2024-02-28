using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class SingleLobbyListUI : MonoBehaviour
{
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    
    private Lobby _associated;

    private void Awake()
    {
        joinLobbyButton.onClick.AddListener(() =>
        {
            GameLobbyManager.Instance.JoinLobbyById(_associated.Id);
        });
    }

    public void UpdateVisuals(Lobby associated)
    {
        _associated = associated;

        lobbyNameText.text = _associated.Name;
    }
}
