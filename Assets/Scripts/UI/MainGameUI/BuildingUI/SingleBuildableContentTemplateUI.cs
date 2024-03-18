using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
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
        // TODO BUILDING LOGIC BASED ON :
        // - RESOURCES AVAILABLE
        // - IF THERE'S ALREADY A TOWER ON THE CELL
        SynchronizeBuilding.Instance.SpawnBuildableObject(_associatedBuildableObjectSo, _associatedBuildableCell);
    }
}
