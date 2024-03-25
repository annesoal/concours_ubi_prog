using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using TMPro;
using UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
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
        
        upArrowButton.onClick.AddListener(ChangeSelectedCellUp);
        downArrowButton.onClick.AddListener(ChangeSelectedCellDown);
        leftArrowButton.onClick.AddListener(ChangeSelectedCellLeft);
        rightArrowButton.onClick.AddListener(ChangeSelectedCellRight);
        
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
        DestroyLastPreview();
        
        _trapPreview = Instantiate(_trapSO.visuals);
        _trapPreview.GetComponent<BuildableObjectVisuals>().ShowPreview();

        Vector3 previewDestination = TilingGrid.CellPositionToLocal(_selectedCell);
        
        _trapPreview.transform.position = previewDestination;
        
        AddHighlighter(previewDestination);
    }

    private void DestroyLastPreview()
    {
        Destroy(_trapPreview);
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
        
        UpdateUI();
    }

    private void ChangeSelectedCellUp()
    {
        ChangeSelectedCell(Vector2Int.up);
    }
    
    private void ChangeSelectedCellDown()
    {
        ChangeSelectedCell(Vector2Int.down);
    }
    
    private void ChangeSelectedCellRight()
    {
        ChangeSelectedCell(Vector2Int.right);
    }
    
    private void ChangeSelectedCellLeft()
    {
        ChangeSelectedCell(Vector2Int.left);
    }

    private void ChangeSelectedCell(Vector2Int direction)
    {
        Cell nextCell = TilingGrid.grid.GetCell(_selectedCell.position + direction);

        if (nextCell.Has(BlockType.EnnemyWalkable) && !nextCell.Has(BlockType.Buildable))
        {
            _selectedCell = nextCell;
            UpdateUI();
        }
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

        RemoveHighlighter();
    }

    private void RemoveHighlighter()
    {
        Destroy(_currentHighlighter.gameObject);
    }
}
