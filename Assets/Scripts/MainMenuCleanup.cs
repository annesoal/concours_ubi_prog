using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanup : MonoBehaviour
{
    private void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (GameMultiplayerManager.Instance != null)
        {
            Destroy(GameMultiplayerManager.Instance.gameObject);
        }

        if (GameLobbyManager.Instance != null)
        {
            Destroy(GameLobbyManager.Instance.gameObject);
        }
    }
}
