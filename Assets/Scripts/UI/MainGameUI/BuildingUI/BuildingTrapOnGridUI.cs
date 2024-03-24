using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UI;
using UnityEngine;

public class BuildingTrapOnGridUI : MonoBehaviour
{
    private void Awake()
    {
        TilingGrid.grid.GetEnemyWalkableCells();
    }

    public void Show(BuildableObjectSO trapSO)
    {
        // TODO
        Debug.Log(trapSO.objectName);
        BasicShowHide.Show(gameObject);
    }
}
