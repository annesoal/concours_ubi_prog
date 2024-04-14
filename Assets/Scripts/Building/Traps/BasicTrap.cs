using System;
using System.Collections;
using System.Collections.Generic;
using Building.Traps;
using Enemies;
using Grid;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Stupefies enemy on collision.
/// </summary>
public class BasicTrap : BaseTrap
{
    public static int SetCost; 
    public static int StunDuration;
    public static int TrapRange;

    private int _cost = SetCost;
    private int _TrapRange = TrapRange;
    public override int Cost { get => _cost; set => _cost = value ; }
    public override int Range { get => _TrapRange; set => _TrapRange = value; }


    protected override void ActivateTrapBehaviour(Enemy enemy)
    {
        enemy.SetAsStupefied(StunDuration);
    }

    public override TrapPlayInfo GetPlay()
    {
        Cell currentCell = TilingGrid.grid.GetCell(transform.position);
        Enemy enemyGO = currentCell.GetEnemy();
        if (enemyGO == null)
        {
            return new TrapPlayInfo()
            {
                isTrigger = false,
            };
        }

        var trapPlayInfo = new TrapPlayInfo();
        trapPlayInfo.isTrigger = true;
        CleanUp();

        List<Cell> cells = TilingGrid.grid.GetCellsInRadius(currentCell, Range);
        trapPlayInfo.enemiesAffectedInfo = new();
        foreach (var cell in cells)
        {
            Enemy enemyToStun = cell.GetEnemy();
            if (enemyToStun != null)
            {
                enemyToStun.SetAsStupefied(StunDuration);
                var enemyAffected = new EnemyAffectedInfo()
                {
                    enemy = enemyToStun,
                };
                trapPlayInfo.enemiesAffectedInfo.Add(enemyAffected);
            }
        }

        return trapPlayInfo;
    }

    public override IEnumerator PlayAnimation(TrapPlayInfo trapPlayInfo)
    {
        
        HasFinishedAnimation = false;

        if (trapPlayInfo.isTrigger == false)
        {
            HasFinishedAnimation = true;
            yield break;
        }
        visuals.SetActive(false); 

        foreach (var enemyAffectedInfo in trapPlayInfo.enemiesAffectedInfo)
        {
            StartCoroutine(enemyAffectedInfo.enemy.PushBackAnimation(transform.position));
        }
        HasFinishedAnimation = true;
        Destroy(this.gameObject);

    }
}
