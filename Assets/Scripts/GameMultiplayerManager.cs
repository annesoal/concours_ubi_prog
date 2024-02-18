using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameMultiplayerManager : NetworkBehaviour
{
    public static GameMultiplayerManager Instance { get; private set; }
    
    public static int MAX_NUMBER_OF_PLAYERS = 2;

    private void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
