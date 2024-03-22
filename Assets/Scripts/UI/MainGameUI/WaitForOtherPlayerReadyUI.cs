using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class WaitForOtherPlayerReadyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waitForOtherPlayerText;
    [SerializeField] private TextMeshProUGUI areYouReadyText;
    [SerializeField] private Button readyButton;

    private void Awake()
    {
        readyButton.onClick.AddListener(() =>
        {
            TowerDefenseManager.Instance.SetPlayerReadyToPlay();
            
            areYouReadyText.gameObject.SetActive(false);
            waitForOtherPlayerText.gameObject.SetActive(true);
        });
        
        readyButton.Select();
    }

    private void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentState;
    }

    private void TowerDefenseManager_OnCurrentState
        (object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs onCurrentStateChangedEventArgs)
    {
        BasicShowHide.Hide(gameObject);
    }
}
