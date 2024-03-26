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

    [Header("Camera")]
    [SerializeField] private CameraController cameraController;
    
    [Header("Other")]
    [SerializeField] private TextMeshProUGUI errorText;

    private LinkedList<Cell> _buildableCells;
    private LinkedListNode<Cell> _selectedCell;
    private BuildableObjectSO _towerToBuild;

    private GameObject _preview;

    private void Awake()
    {
        closeUIButton.onClick.AddListener(() =>
        {
            InputManager.Instance.EnablePlayerInputMap();
            
            Hide();
            
            CentralizedInventory.Instance.ClearAllMaterialsCostUI();
        });
        
        buildButton.onClick.AddListener(OnBuildButtonClicked);
        
        rightArrow.onClick.AddListener(ChangeSelectedCellRight);
        leftArrow.onClick.AddListener(ChangeSelectedCellLeft);
    }

    private void Start()
    {
        SynchronizeBuilding.Instance.OnBuildingBuilt += SynchronizeBuilding_OnBuildingBuilt;
        
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
        UpdateSelectedCell(e.BuildingPosition);
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

        cameraController.MoveCameraToPosition(previewPosition);

        _preview = Instantiate(_towerToBuild.visuals);

        BuildableObjectVisuals previewVisualsComponent = _preview.GetComponent<BuildableObjectVisuals>();
        
        previewVisualsComponent.ShowPreview();

        _preview.transform.position = previewPosition;
    }

    private void HidePreview()
    {
        if (_preview != null)
        {
            Destroy(_preview);
            _preview = null;
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
