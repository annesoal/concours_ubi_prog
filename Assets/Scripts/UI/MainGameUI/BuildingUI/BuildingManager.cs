using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    private List<Cell> _buildableCells;

    private TowerSO _towerSelected;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _buildableCells = TilingGrid.grid.GetBuildableCells();
        
        SingleTowerSelectUI.OnAnySingleTowerSelectUISelected += SingleTowerSelectUI_OnAnySingleTowerSelectUISelected;
    }

    private void SingleTowerSelectUI_OnAnySingleTowerSelectUISelected(object sender, SingleTowerSelectUI.TowerData e)
    {
        _towerSelected = e.towerInfos;
    }

    public List<Cell> GetBuildableCellsList()
    {
        return _buildableCells;
    }
}
