using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;

    private void Start()
    {
        lobbyNameText.text = GameLobbyManager.Instance.GetLobbyName();
        lobbyCodeText.text = GameLobbyManager.Instance.GetLobbyCode();
    }
}
