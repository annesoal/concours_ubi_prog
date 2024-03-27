using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using UnityEngine;

public class TestingTest : MonoBehaviour, ITopOfCell
{
    void Start()
    {
        TilingGrid.grid.PlaceObjectAtPositionOnGrid( this.gameObject,gameObject.transform.position);
    }

    public new TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Enemy;
    }

    public GameObject ToGameObject()
    {
        return this.gameObject;
    }
}
