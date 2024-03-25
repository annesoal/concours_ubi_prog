using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using TMPro;
using UI;
using Unity.Mathematics;
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
    [SerializeField] private GameObject highlighterGameObject;
    
    private List<Cell> _enemyWalkableCells;
    private Cell _selectedCell;

    private BuildableObjectSO _trapSO;
    private GameObject _trapPreview;
    private GameObject _currentHighlighter;

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
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        
        SynchronizeBuilding.Instance.OnBuildingBuilt += SynchronizeBuilding_OnBuildingBuilt;
        
        BasicShowHide.Hide(gameObject);
    }

    public void Show(BuildableObjectSO trapSO)
    {
        Debug.Log(trapSO.objectName);
        _trapSO = trapSO;
        
        _enemyWalkableCells = TilingGrid.grid.GetEnemyWalkableCells();
        _selectedCell = _enemyWalkableCells[0];
        
        UpdateUI();
        
        BasicShowHide.Show(gameObject);
    }

    private const string ALREADY_HAS_BUILDING_ERROR = "ALREADY HAS A BUILDING";
    
    private void UpdateUI()
    {
        if (_selectedCell.HasNotBuildingOnTop())
        {
            BasicShowHide.Hide(errorText.gameObject);
            ShowPreviewOnSelectedCell();
        }
        else
        {
            DestroyPreview();
            
            errorText.text = ALREADY_HAS_BUILDING_ERROR;
            BasicShowHide.Show(errorText.gameObject);
        }
    }
    
    private void ShowPreviewOnSelectedCell()
    {
        GameObject trapVisuals = Instantiate(_trapSO.visuals);
        trapVisuals.GetComponent<BuildableObjectVisuals>().ShowPreview();

        TilingGrid.grid.PlaceObjectAtPositionOnGrid(trapVisuals, _selectedCell.position);
        AddHighlighter(TilingGrid.CellPositionToLocal(_selectedCell));
    }
    
    private void AddHighlighter(Vector3 position)
    {
        Destroy(_currentHighlighter);
        _currentHighlighter = Instantiate(highlighterGameObject, position, quaternion.identity);
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
    
    private void TowerDefenseManager_OnCurrentStateChanged(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == TowerDefenseManager.State.EnvironmentTurn)
        {
            DestroyPreview();
        }
    }

    private void DestroyPreview()
    {
        Destroy(_trapPreview);
    }
}
