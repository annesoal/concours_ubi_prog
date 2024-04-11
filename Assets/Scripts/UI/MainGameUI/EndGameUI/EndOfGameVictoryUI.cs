using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using UI;
using Unity.Netcode;
using UnityEngine;

public class EndOfGameVictoryUI : MonoBehaviour
{
    [SerializeField] private AmuletChoicesUI amuletChoicesUI;
    [SerializeField] private GameObject HostIsChosingUI;
    
    private void Start()
    {
        TowerDefenseManager.Instance.OnVictory += TowerDefenseManager_OnVictory;

        amuletChoicesUI.SetVisuals(TowerDefenseManager.Instance.nextLevelDataSo.AmuletChoiceAtEnd);
        
        amuletChoicesUI.OnAmuletChosen += AmuletChoicesUI_OnAmuletChosen;
        
        BasicShowHide.Hide(gameObject);
    }

    private void AmuletChoicesUI_OnAmuletChosen(object sender, AmuletChoicesUI.OnAmuletChosenEventArgs e)
    {
        AmuletSelector.PlayerAmuletSelection = e.AmuletChosen;
        
        Loader.LoadNetwork(TowerDefenseManager.Instance.nextLevelDataSo.nextLevelScene);
    }

    private void TowerDefenseManager_OnVictory(object sender, EventArgs e)
    {
        BasicShowHide.Show(gameObject);

        TryShowHostIsChosingUI();

        amuletChoicesUI.InitiateFirstButtonSelect();
    }

    private void TryShowHostIsChosingUI()
    {
        if (! NetworkManager.Singleton.IsServer)
        {
            BasicShowHide.Show(HostIsChosingUI);
        }
        else
        {
            BasicShowHide.Hide(HostIsChosingUI);
        }
    }
}
