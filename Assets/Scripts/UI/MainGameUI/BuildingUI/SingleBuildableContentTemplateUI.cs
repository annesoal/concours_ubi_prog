using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using UnityEngine;
using UnityEngine.UI;

public class SingleBuildableContentTemplateUI : MonoBehaviour
{
    [SerializeField] private Button selectionButton;

    private void Awake()
    {
        selectionButton.onClick.AddListener(BuildObjectOnButtonClick);
    }

    private Cell _associatedBuildableCell;
    private BuildableObjectSO _associatedBuildableObjectSo;
    
    public void SetTemplateInfos(Cell buildableCell, BuildableObjectSO buildableObjectSo)
    {
        _associatedBuildableCell = buildableCell;
        _associatedBuildableObjectSo = buildableObjectSo;
    }

    private void BuildObjectOnButtonClick()
    {
        // Si l'autre joueur modifie la cell entre-temps, on veut la cell la plus à jour au moment de la construction.
        _associatedBuildableCell = TilingGrid.grid.GetCell(_associatedBuildableCell.position);
        
        // TODO BUILDING LOGIC BASED ON :
        // - RESOURCES AVAILABLE
        if (HasNotBuildingOnTop(_associatedBuildableCell.ObjectsTopOfCell))
        {
            SynchronizeBuilding.Instance.SpawnBuildableObject(_associatedBuildableObjectSo, _associatedBuildableCell);
            
            _associatedBuildableCell = TilingGrid.grid.GetCell(_associatedBuildableCell.position);
        }
    }

    private bool HasNotBuildingOnTop(List<ITopOfCell> objectsOnTopOfCell)
    {
        foreach (ITopOfCell objectOnTop in objectsOnTopOfCell)
        {
            if (objectOnTop.GetType() == TypeTopOfCell.Building)
            {
                return false;
            }
        }

        return true;
    }
}
