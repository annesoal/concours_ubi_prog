using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTrapOnGridUI : MonoBehaviour
{
    [Header("Build")]
    [SerializeField] private Button buildButton;
    
    [Header("Arrows button")]
    [SerializeField] private Button upArrowButton;
    [SerializeField] private Button downArrowButton;
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private Button leftArrowButton;

    [Header("Other")]
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI errorText;
    
    private List<Cell> _enemyWalkableCells;
    private Cell _selectedCell;

    private BuildableObjectSO _trapSO;

    private void Awake()
    {
        buildButton.onClick.AddListener(BuildTrapOnButtonClick);
        
        closeButton.onClick.AddListener(() =>
        {
            InputManager.Instance.EnablePlayerInputMap();
            
            BasicShowHide.Hide(gameObject);
            
            CentralizedInventory.Instance.ClearAllMaterialsCostUI();
        });
    }
    
    private void Start()
    {
        SynchronizeBuilding.Instance.OnBuildingBuilt += SynchronizeBuilding_OnBuildingBuilt;
        
        BasicShowHide.Hide(gameObject);
    }

    public void Show(BuildableObjectSO trapSO)
    {
        Debug.Log(trapSO.objectName);
        _trapSO = trapSO;
        
        _enemyWalkableCells = TilingGrid.grid.GetEnemyWalkableCells();
        _selectedCell = _enemyWalkableCells[0];
        
        BasicShowHide.Show(gameObject);
    }
    
    private void BuildTrapOnButtonClick()
    {
        if (IsAbleToBuild())
        {
            SynchronizeBuilding.Instance.SpawnBuildableObject(_trapSO, _selectedCell);
            
            UpdateSelectedCell();
        }
    }

    private bool IsAbleToBuild()
    {
        _selectedCell = TilingGrid.grid.GetCell(_selectedCell.position);
        
        return _selectedCell.HasNotBuildingOnTop() &&
               CentralizedInventory.Instance.HasResourcesForBuilding(_trapSO);
    }
    
    private void SynchronizeBuilding_OnBuildingBuilt(object sender, SynchronizeBuilding.OnBuildingBuiltEventArgs e)
    {
        if (e.BuildingPosition == _selectedCell.position)
        {
            UpdateSelectedCell();
        }
    }
    
    private void UpdateSelectedCell()
    {
        _selectedCell = TilingGrid.grid.GetCell(_selectedCell.position);
        
        UpdatePreviewUI();
    }
    
    private void UpdatePreviewUI()
    {
        // TODO
    }
}
