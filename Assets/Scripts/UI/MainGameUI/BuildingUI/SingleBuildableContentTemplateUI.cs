using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.UI;

public class SingleBuildableContentTemplateUI : MonoBehaviour
{
    [SerializeField] private Button selectionButton;

    private void Awake()
    {
        selectionButton.onClick.AddListener(() =>
        {
            // TODO BUILDING LOGIC BASED ON :
            // - RESOURCES AVAILABLE
            // - IF THERE'S ALREADY A TOWER ON THE CELL
        });
    }

    public void AddBuildableCell(Cell buildableCell, TowerSO towerSo)
    {
        
    }
}
