using System;
using System.Collections;
using System.Collections.Generic;
using Grid; using Grid.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleBuildableContentTemplateUI : MonoBehaviour
{
    [SerializeField] private Button selectionButton;

    private void Awake()
    {
        selectionButton.onClick.AddListener(BuildObjectOnButtonClick);
    }

    private void Start()
    {
        SynchronizeBuilding.Instance.OnBuildingBuilt += SynchronizeBuilding_OnBuildingBuilt;
    }

    private Cell _associatedBuildableCell;
    private BuildableObjectSO _associatedBuildableObjectSo;
    
    public void SetTemplateInfos(Cell buildableCell, BuildableObjectSO buildableObjectSo)
    {
        _associatedBuildableCell = buildableCell;
        _associatedBuildableObjectSo = buildableObjectSo;
    }
    
    public void SelectThisSelectionButton()
    {
        selectionButton.Select();
    }

    private void BuildObjectOnButtonClick()
    {
        if (IsAbleToBuild())
        {
            SynchronizeBuilding.Instance.SpawnBuildableObject(_associatedBuildableObjectSo, _associatedBuildableCell);
            
            UpdateAssociatedCell();
        }
    }

    private bool IsAbleToBuild()
    {
        // Si l'autre joueur modifie la cell entre-temps, on veut la cell la plus à jour au moment de la construction.
        _associatedBuildableCell = TilingGrid.grid.GetCell(_associatedBuildableCell.position);
        
        return _associatedBuildableCell.HasNotBuildingOnTop() &&
               CentralizedInventory.Instance.HasResourcesForBuilding(_associatedBuildableObjectSo);
    }

    public Vector2Int GetAssociatedCellPosition()
    {
        return _associatedBuildableCell.position;
    }

    public BuildableObjectSO GetAssociatedBuildableObjectSO()
    {
        return _associatedBuildableObjectSo;
    }

    public bool AssociatedCellHasNotBuildingOnTop()
    {
        _associatedBuildableCell = TilingGrid.grid.GetCell(_associatedBuildableCell.position);

        return _associatedBuildableCell.HasNotBuildingOnTop();
    }

    private void SynchronizeBuilding_OnBuildingBuilt(object sender, SynchronizeBuilding.OnBuildingBuiltEventArgs e)
    {
        if (_associatedBuildableCell.position == e.BuildingPosition)
        {
            UpdateAssociatedCell();
        }
    }

    private void UpdateAssociatedCell()
    {
        _associatedBuildableCell = TilingGrid.grid.GetCell(_associatedBuildableCell.position);
        
        UpdatePreviewUI();
    }

    private void UpdatePreviewUI()
    {
        SingleBuildableContentButtonUI buttonUI = selectionButton.GetComponent<SingleBuildableContentButtonUI>();

        buttonUI.UpdatePreviewAfterBuilding();
    }
}
