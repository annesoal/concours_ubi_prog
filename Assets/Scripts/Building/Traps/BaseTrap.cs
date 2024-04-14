using System;
using System.Collections;
using System.Collections.Generic;
using Building.Traps;
using Enemies;
using Grid;
using Grid.Interface;
using UnityEngine;

public abstract class BaseTrap : BuildableObject
{
    [SerializeField] private BuildableObjectVisuals trapVisuals;
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject visuals;
    public bool HasFinishedAnimation;

    public abstract int Range { get; set; }
    protected abstract void ActivateTrapBehaviour(Enemy enemy);

    public abstract TrapPlayInfo GetPlay();
    public abstract IEnumerator PlayAnimation(TrapPlayInfo trapPlayInfo); 
    public override void Build(Vector2Int positionToBuild)
    {
        trapVisuals.HidePreview();
        
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, positionToBuild);
       
        TrapManager.Instance.trapsInGame.Add(this);
    }

    public override TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Building;
    }

    public override bool IsWalkable()
    {
        return true;
    }

    public void CleanUp()
    {
        TrapManager.Instance.trapsInGame.Remove(this);
        TilingGrid.RemoveElement(this.gameObject, this.transform.position);
    }
}
