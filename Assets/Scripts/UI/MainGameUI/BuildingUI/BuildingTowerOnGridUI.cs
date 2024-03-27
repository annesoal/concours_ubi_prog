using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingTowerOnGridUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button closeUIButton;
    [SerializeField] private Button buildButton;
    [SerializeField] private Button rightArrow;
    [SerializeField] private Button leftArrow;

    [Header("Other")]
    [SerializeField] private TextMeshProUGUI errorText;

    private LinkedList<Cell> _buildableCells;
    private LinkedListNode<Cell> _selectedCell;
    private BuildableObjectSO _towerToBuild;

    private GameObject _preview;

    private void Awake()
    {
        closeUIButton.onClick.AddListener(CloseUI);
        
        buildButton.onClick.AddListener(OnBuildButtonClicked);
        
        rightArrow.onClick.AddListener(ChangeSelectedCellRight);
        leftArrow.onClick.AddListener(ChangeSelectedCellLeft);
    }

    private void Start()
    {
        SynchronizeBuilding.Instance.OnBuildingBuilt += SynchronizeBuilding_OnBuildingBuilt;

        InputManager.Instance.OnUserInterfaceCancelPerformed += InputManager_OnUserInterfaceCancelPerformed;
        InputManager.Instance.OnUserInterfaceLeftPerformed += InputManager_OnUserInterfaceLeftPerformed;
        InputManager.Instance.OnUserInterfaceRightPerformed += InputManager_OnUserInterfaceRightPerformed;
        
        _buildableCells = TilingGrid.grid.GetBuildableCells();
        
        _selectedCell = _buildableCells.First;
        
        BasicShowHide.Hide(gameObject);
    }

    public void Show(BuildableObjectSO buildableObjectSO)
    {
        InputManager.Instance.DisablePlayerInputMap();

        _towerToBuild = buildableObjectSO;
        
        BasicShowHide.Show(gameObject);
        
        UpdateUI();
        
        buildButton.Select();
    }

    public void Hide()
    {
        HidePreview();
        
        BasicShowHide.Hide(gameObject);
    }
    
    private void OnBuildButtonClicked()
    {
        Debug.Log("BUILD BUTTON CLICKED !");
        
        if (IsAbleToBuild())
        {
            SynchronizeBuilding.Instance.SpawnBuildableObject(_towerToBuild, _selectedCell.Value);
            
            UpdateSelectedCell(_selectedCell.Value.position);
        }
    }

    private bool IsAbleToBuild()
    {
        _selectedCell.Value = TilingGrid.grid.GetCell(_selectedCell.Value.position);
        
        return _selectedCell.Value.HasNotBuildingOnTop() &&
               CentralizedInventory.Instance.HasResourcesForBuilding(_towerToBuild);
    }

    private void SynchronizeBuilding_OnBuildingBuilt(object sender, SynchronizeBuilding.OnBuildingBuiltEventArgs e)
    {
        if (gameObject.activeSelf)
        {
            UpdateSelectedCell(e.BuildingPosition);
        }
    }

    private const string ALREADY_HAS_BUILDING_ERROR = "ALREADY HAS A BUILDING";
    
    private void UpdateSelectedCell(Vector2Int cellToUpdate)
    {
        if (_selectedCell.Value.position == cellToUpdate)
        {
            _selectedCell.Value = TilingGrid.grid.GetCell(cellToUpdate);
        }
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        CameraController.Instance.MoveCameraToPosition
            (TilingGrid.GridPositionToLocal(_selectedCell.Value.position));
        
        if (_selectedCell.Value.HasNotBuildingOnTop())
        {
            BasicShowHide.Hide(errorText.gameObject);
            
            ShowPreview();
        }
        else
        {
            errorText.text = ALREADY_HAS_BUILDING_ERROR;
            BasicShowHide.Show(errorText.gameObject);
            
            HidePreview();
        }
    }

    private void ShowPreview()
    {
        HidePreview();

        Vector3 previewPosition = TilingGrid.GridPositionToLocal(_selectedCell.Value.position);

        _preview = Instantiate(_towerToBuild.visuals);

        BuildableObjectVisuals previewVisualsComponent = _preview.GetComponent<BuildableObjectVisuals>();
        
        previewVisualsComponent.ShowPreview(previewPosition);
    }

    private void HidePreview()
    {
        if (_preview != null)
        {
            Destroy(_preview);
            _preview = null;
        }
    }
    
    private void InputManager_OnUserInterfaceRightPerformed(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ChangeSelectedCellRight();
        }
    }
    
    private void InputManager_OnUserInterfaceCancelPerformed(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            CloseUI();
        }
    }

    private void CloseUI()
    {
        InputManager.Instance.EnablePlayerInputMap();
            
        Hide();
            
        CentralizedInventory.Instance.ClearAllMaterialsCostUI();
    }
    
    private void InputManager_OnUserInterfaceLeftPerformed(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ChangeSelectedCellLeft();
        }
    }
    
    private void ChangeSelectedCellRight()
    {
        _selectedCell = _selectedCell.Next;

        if (_selectedCell == null)
        {
            _selectedCell = _buildableCells.First;
        }
        
        UpdateSelectedCell(_selectedCell.Value.position);
    }
    
    private void ChangeSelectedCellLeft()
    {
        _selectedCell = _selectedCell.Previous;

        if (_selectedCell == null)
        {
            _selectedCell = _buildableCells.Last;
        }
        
        UpdateSelectedCell(_selectedCell.Value.position);
    }

}
