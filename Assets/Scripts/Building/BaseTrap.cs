using System.Collections;
using System.Collections.Generic;
using Grid.Interface;
using UnityEngine;

public abstract class BaseTrap : MonoBehaviour, IBuildable, ITopOfCell
{
    public void Build(Vector2Int positionToBuild)
    {
        throw new System.NotImplementedException();
    }

    public BuildableObjectSO GetBuildableObjectSO()
    {
        throw new System.NotImplementedException();
    }

    public TypeTopOfCell GetType()
    {
        throw new System.NotImplementedException();
    }

    public GameObject ToGameObject()
    {
        throw new System.NotImplementedException();
    }
}
