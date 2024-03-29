using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using UnityEngine;

public abstract class BaseTrap : BuildableObject
{
    [SerializeField] private BuildableObjectVisuals trapVisuals;
    
    public override void Build(Vector2Int positionToBuild)
    {
        trapVisuals.HidePreview();
        
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, positionToBuild);
    }

    public override TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Building;
    }

    public override int Cost { get; set; }
}
