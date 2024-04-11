using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonCharacterSelectUI : MonoBehaviour
{
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                GameMultiplayerManager.Instance.NullifyPlayerDataNetworkList();
            }
            
            Loader.ReturnToMainMenuClean();
        });
    }
}
