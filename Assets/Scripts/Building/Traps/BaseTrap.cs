using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Grid;
using Grid.Interface;
using UnityEngine;

public abstract class BaseTrap : BuildableObject
{
    static public int baseCost;
    [SerializeField] private BuildableObjectVisuals trapVisuals;
    [SerializeField] private int damage;

    protected virtual void Start()
    {
        Enemy.OnAnyEnemyMoved += Enemy_OnAnyEnemyMoved;
    }

    protected abstract void ActivateTrapBehaviour(Enemy enemy);
    
    public override void Build(Vector2Int positionToBuild)
    {
        trapVisuals.HidePreview();
        
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, positionToBuild);
    }

    public override TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Building;
    }

    private int _cost = baseCost;
    public override int Cost { get => _cost; set => value=_cost; }

    private void Enemy_OnAnyEnemyMoved(object sender, EventArgs e)
    {
        Cell trapCell = TilingGrid.grid.GetCell(TilingGrid.LocalToGridPosition(transform.position));

        List<Enemy> enemiesOnTrapCell = trapCell.GetEnemies();

        if (enemiesOnTrapCell.Count != 0)
        {
            foreach (Enemy toDamage in enemiesOnTrapCell)
            {
                ActivateTrapBehaviour(toDamage);
            }
            
            TilingGrid.RemoveElement(gameObject, transform.position);
        
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        Enemy.OnAnyEnemyMoved -= Enemy_OnAnyEnemyMoved;
    }
}