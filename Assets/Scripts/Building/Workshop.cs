using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Blocks;
using UnityEngine;

public class Workshop : MonoBehaviour
{
    [SerializeField] private Transform blockUnder;
    
    void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
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
}
