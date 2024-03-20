using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuUI : MonoBehaviour
{
    private void Awake()
    {
        showBuildingMenuButton.onClick.AddListener(() =>
        {
            InputManager.Instance.DisablePlayerInputMap();
            
            circularLayout.ShowLayout();
        });
    }

    private void Start()
    {
        BasicShowHide.Hide(showBuildingMenuButton.gameObject);
        
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        
        Workshop.OnAnyWorkshopNearPlayer += Workshop_OnAnyWorkshopNearPlayer;
        
        SingleBuildableObjectSelectUI.OnAnySingleBuildableObjectSelectUISelected +=
            SingleTowerSelectUI_OnAnySingleBuildableObjectSelectUISelected;
        
        circularLayout.HideLayout();
        
        UpdateLayoutVisuals();
    }

    private void UpdateLayoutVisuals()
    {
        foreach (BuildableObjectSO buildableObjectSo in SynchronizeBuilding.Instance.GetAllBuildableObjectSo().list)
        {
            circularLayout.AddObjectToLayout(buildableObjectSo);
        }
    }

    [SerializeField] private Button showBuildingMenuButton;
    [SerializeField] private CircularLayoutUI circularLayout;
    
    private void TowerDefenseManager_OnCurrentStateChanged(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue != TowerDefenseManager.State.TacticalPause)
        {
            BasicShowHide.Hide(showBuildingMenuButton.gameObject);
            BasicShowHide.Hide(circularLayout.gameObject);
        }
    }
    
    private void Workshop_OnAnyWorkshopNearPlayer(object sender, EventArgs e)
    {
        BasicShowHide.Show(showBuildingMenuButton.gameObject);
    }


    [SerializeField] private BuildingObjectOnGridUI buildingTowerOnGridUI;
    
    private void SingleTowerSelectUI_OnAnySingleBuildableObjectSelectUISelected
        (object sender, SingleBuildableObjectSelectUI.BuildableObjectData e)
    {
        BasicShowHide.Hide(circularLayout.gameObject);
        buildingTowerOnGridUI.Show(e.buildableObjectInfos);
    }

    private bool PlayerIsOnBuildingBlock()
    {
        Vector2Int playerPositionOnGrid = TilingGrid.LocalToGridPosition(Player.LocalInstance.transform.position);

        Cell underPlayerCell = TilingGrid.grid.GetCell(playerPositionOnGrid);

        return underPlayerCell.IsOf(BlockType.Buildable);
    }
}
