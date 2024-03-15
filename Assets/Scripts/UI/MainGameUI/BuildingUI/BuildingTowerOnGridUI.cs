using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTowerOnGridUI : MonoBehaviour
{
    [SerializeField] private Button closeUIButton;
    
    private TowerSO _selectedTowerSo;

    private void Awake()
    {
        closeUIButton.onClick.AddListener(() =>
        {
            // TODO enable back player input
            
            BasicShowHide.Hide(gameObject);
        });
    }

    public void Show(TowerSO selectedTowerSo)
    {
        // TODO disable player input
        
        _selectedTowerSo = selectedTowerSo;
        
        BasicShowHide.Show(gameObject);
    }

    private void Update()
    {
        // TODO GetPlayerInput and select tower (cycling across potential building position)
    }
}
