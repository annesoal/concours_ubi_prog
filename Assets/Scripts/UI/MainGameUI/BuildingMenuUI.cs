using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class BuildingMenuUI : MonoBehaviour
{
    /// <summary>
    /// Ensemble des tours pouvant être construites par les joueurs dans le niveau.
    /// </summary>
    [SerializeField] private TowersListSO availableTowersListSO;

    private void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
    }

    private void TowerDefenseManager_OnCurrentStateChanged(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == TowerDefenseManager.State.TacticalPause)
        {
            if (PlayerIsOnBuildingBlock())
            {
                // Show building UI (button to show it at least)
            }
        }
        else
        {
            // Hide UI
        }
    }

    private bool PlayerIsOnBuildingBlock()
    {
        Vector2Int playerPositionOnGrid = TilingGrid.LocalToGridPosition(Player.LocalInstance.transform.position);

        Cell underPlayerCell = TilingGrid.grid.GetCell(playerPositionOnGrid);

        return underPlayerCell.IsOf(BlockType.Buildable);
    }
}
