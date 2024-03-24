using System.Collections;
using System.Collections.Generic;
using Grid.Interface;
using UnityEngine;

public abstract class BaseTrap : BuildingObject
{
    public override void Build(Vector2Int positionToBuild)
    {
        throw new System.NotImplementedException();
    }

    public override TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Building;
    }
}
