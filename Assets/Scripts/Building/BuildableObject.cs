using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Interface;
using UnityEngine;

public abstract class BuildableObject : MonoBehaviour, IBuildable, ITopOfCell
{
    public abstract int Cost { get; set; }
    [field: Header("Buildable Object")]
    [SerializeField] protected BuildableObjectSO buildableObjectSO;

    public void Awake()
    {
        var pair = buildableObjectSO.materialAndQuantityPairs[0];
        pair.quantityOfMaterialRequired = Cost;
        buildableObjectSO.materialAndQuantityPairs[0] = pair;
    }

    public abstract void Build(Vector2Int positionToBuild);

    public  BuildableObjectSO GetBuildableObjectSO()
    {
        return buildableObjectSO;
    }

    public new abstract TypeTopOfCell GetType();

    public GameObject ToGameObject()
    {
        return gameObject;
    }

    public bool IsWalkable()
    {
        return buildableObjectSO.type == BuildableObjectSO.TypeOfBuildableObject.Trap;
    }
}
