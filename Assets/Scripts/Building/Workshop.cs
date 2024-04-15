using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Blocks;
using Grid.Interface;
using UnityEngine;

public class Workshop : MonoBehaviour, ITopOfCell
{
    [SerializeField] private Transform blockUnder;
    
    void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        Cell cell = TilingGrid.grid.GetCell(blockUnder.position);
        cell.ObjectsTopOfCell.Add(this);
        TilingGrid.grid.UpdateCell(cell);
    }

    public static event EventHandler OnAnyWorkshopNearPlayer;
    
    private void TowerDefenseManager_OnCurrentStateChanged
        (object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == TowerDefenseManager.State.TacticalPause)
        {
            if (PlayerIsOnSameCell())
            {
                OnAnyWorkshopNearPlayer?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private bool PlayerIsOnSameCell()
    {
        return 
            TilingGrid.LocalToGridPosition(blockUnder.transform.position) == 
            TilingGrid.LocalToGridPosition(Player.LocalInstance.transform.position);
    }

    public static void ResetStaticData()
    {
        OnAnyWorkshopNearPlayer = null;
    }

    public TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Building;
    }

    public GameObject ToGameObject()
    {
        return this.gameObject;
    }
}
