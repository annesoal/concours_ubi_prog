using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using UnityEngine;

public abstract class BuildableObject : MonoBehaviour, IBuildable, ITopOfCell
{
    public abstract int Cost { get; set; }
    [field: Header("Buildable Object")]
    [SerializeField] protected BuildableObjectSO buildableObjectSO;

    public abstract void Build(Vector2Int positionToBuild);

    public  BuildableObjectSO GetBuildableObjectSO()
    {
        return buildableObjectSO;
    }

    public abstract void HidePreview();
    public void SynchBuild()
    {
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, this.transform.position);
    }



    public new abstract TypeTopOfCell GetType();

    public GameObject ToGameObject()
    {
        return gameObject;
    }

    public virtual bool IsWalkable()
    {
        return buildableObjectSO.type == BuildableObjectSO.TypeOfBuildableObject.Trap;
    }
}
