using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using UI;
using UnityEngine;

public class EndOfGameVictoryUI : MonoBehaviour
{
    [SerializeField] private AmuletChoicesUI amuletChoicesUI;
    
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
    }
}
