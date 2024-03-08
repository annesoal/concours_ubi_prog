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
            BasicShowHide.Show(circularLayout.gameObject);
        });
    }

    private void Start()
    {
        BasicShowHide.Hide(showBuildingMenuButton.gameObject);
        
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
    }

    [SerializeField] private Button showBuildingMenuButton;
    [SerializeField] private GameObject circularLayout;
    
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

    private bool PlayerIsOnBuildingBlock()
    {
        Vector2Int playerPositionOnGrid = TilingGrid.LocalToGridPosition(Player.LocalInstance.transform.position);

        Cell underPlayerCell = TilingGrid.grid.GetCell(playerPositionOnGrid);

        return underPlayerCell.IsOf(BlockType.Buildable);
    }
}
