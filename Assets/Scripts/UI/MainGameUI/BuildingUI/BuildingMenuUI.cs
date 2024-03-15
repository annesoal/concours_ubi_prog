using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuUI : MonoBehaviour
{
    /// <summary>
    /// Ensemble des tours pouvant être construites par les joueurs dans le niveau.
    /// </summary>
    [SerializeField] private TowersListSO availableTowersListSO;

    private void Awake()
    {
        showBuildingMenuButton.onClick.AddListener(() =>
        {
            circularLayout.ShowLayout();
        });
    }

    private void Start()
    {
        // DEBUG devra être décommenté
        // BasicShowHide.Hide(showBuildingMenuButton.gameObject);
        
        // DEBUG devra être décommenté
        //TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        SingleTowerSelectUI.OnAnySingleTowerSelectUISelected += SingleTowerSelectUI_OnAnySingleTowerSelectUISelected;
        
        circularLayout.HideLayout();
        
        UpdateLayoutVisuals();
    }

    private void UpdateLayoutVisuals()
    {
        foreach (TowerSO towerSo in availableTowersListSO.allTowersList)
        {
            circularLayout.AddObjectToLayout(towerSo);
        }
    }

    [SerializeField] private Button showBuildingMenuButton;
    [SerializeField] private CircularLayoutUI circularLayout;
    
    private void TowerDefenseManager_OnCurrentStateChanged(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == TowerDefenseManager.State.TacticalPause)
        {
            if (PlayerIsOnBuildingBlock())
            {
                BasicShowHide.Show(showBuildingMenuButton.gameObject);
            }
        }
        else
        {
            BasicShowHide.Hide(showBuildingMenuButton.gameObject);
            BasicShowHide.Hide(circularLayout.gameObject);
        }
    }

    [SerializeField] private BuildingTowerOnGridUI buildingTowerOnGridUI;
    
    private void SingleTowerSelectUI_OnAnySingleTowerSelectUISelected(object sender, SingleTowerSelectUI.TowerData e)
    {
        BasicShowHide.Hide(showBuildingMenuButton.gameObject);
        BasicShowHide.Hide(circularLayout.gameObject);
        buildingTowerOnGridUI.Show(e.towerInfos);
    }

    private bool PlayerIsOnBuildingBlock()
    {
        Vector2Int playerPositionOnGrid = TilingGrid.LocalToGridPosition(Player.LocalInstance.transform.position);

        Cell underPlayerCell = TilingGrid.grid.GetCell(playerPositionOnGrid);

        return underPlayerCell.IsOf(BlockType.Buildable);
    }
}
