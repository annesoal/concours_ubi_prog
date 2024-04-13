using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Grid;
using Grid.Interface;
using UnityEngine;

public abstract class BaseTrap : BuildableObject
{
    [SerializeField] private BuildableObjectVisuals trapVisuals;
    [SerializeField] protected Animator animator;

    public static List<BaseTrap> trapsInGame = new();

    public static void PlayBackEnd()
    {
        foreach (var trap in trapsInGame)
        {
             
        }
    }
    
    protected abstract void ActivateTrapBehaviour(Enemy enemy);
    
    public override void Build(Vector2Int positionToBuild)
    {
        trapVisuals.HidePreview();
        
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, positionToBuild);
       
        trapsInGame.Add(this);
    }

    public override TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Building;
    }
    public static void ResetStaticData()
    {
        trapsInGame = new();
    }

}
