using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class DisconnectMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(Loader.ReturnToMainMenuClean);
    }

    private void Start()
    {
        GameMultiplayerManager.Instance.OnHostDisconneted += GameMultiplayerManager_OnHostDisconneted;
            
        BasicShowHide.Hide(gameObject);
    }

    private void GameMultiplayerManager_OnHostDisconneted(object sender, EventArgs e)
    {
        messageText.text = NetworkManager.Singleton.DisconnectReason;

        if (messageText.text == "")
        {
            messageText.text = "Host Disconnected";
        }
        
        BasicShowHide.Show(gameObject);
        mainMenuButton.Select();
    }

    public void Show(string messageToShow)
    {
        messageText.text = messageToShow;
    }

    private void OnDestroy()
    {
        GameMultiplayerManager.Instance.OnHostDisconneted -= GameMultiplayerManager_OnHostDisconneted;
    }
}
