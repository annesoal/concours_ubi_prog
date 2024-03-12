using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class TowerInfoDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Transform towerVisuals;
    
    private void Start()
    {
        SingleTowerSelectUI.OnAnySingleTowerSelectUIHoveredEnter += SingleTowerSelectUI_OnAnySingleTowerSelectUIHoveredEnter;
        SingleTowerSelectUI.OnAnySingleTowerSelectUIHoveredExit += SingleTowerSelectUI_OnAnySingleTowerSelectUIHoveredExit;
    }

    private void SingleTowerSelectUI_OnAnySingleTowerSelectUIHoveredEnter
        (object sender, SingleTowerSelectUI.TowerData e)
    {
        BasicShowHide.Show(gameObject);
        descriptionText.text = e.towerInfos.description;
    }
    
    private void SingleTowerSelectUI_OnAnySingleTowerSelectUIHoveredExit
        (object sender, SingleTowerSelectUI.TowerData e)
    {
        BasicShowHide.Hide(gameObject);
    }

}
