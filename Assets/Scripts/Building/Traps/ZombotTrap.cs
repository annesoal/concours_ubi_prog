using System.Collections;
using System.Collections.Generic;
using Building.Traps;
using Enemies;
using Grid;
using UnityEngine;

/// <summary>
/// "Kills" enemy on collsion.
/// </summary>
public class ZombotTrap : BaseTrap
{
    public static int SetCost; 
    public static int Damage;
    public static int BombRange; 

    private int _cost = SetCost;
    private int _damage = Damage;
    private int _bombRange = BombRange;
    public override int Cost { get => _cost; set => _cost = value ; }
    public int AttackDamage
    {
        get => _damage;
        set => _damage = value;
    }

    public override int Range { get =>_bombRange; set => _bombRange = value; }

    protected override void ActivateTrapBehaviour(Enemy enemy)
    {
        enemy.Damage(Damage);
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
        this.CleanUp();
        

        List<Cell> cells = TilingGrid.grid.GetCellsInRadius(currentCell, Range);
        trapPlayInfo.enemiesAffectedInfo = new List<EnemyAffectedInfo>();
        foreach (var cell in cells)
        {
            Enemy enemyToDamage = cell.GetEnemy();
            if (enemyToDamage != null)
            {
                var remainingHP = enemyToDamage.Damage(AttackDamage);
                var enemyAffected = new EnemyAffectedInfo();
                if (remainingHP <= 0)
                {
                    enemyAffected.enemy = enemyToDamage;
                    enemyAffected.shouldKill = true;
                    enemyToDamage.CleanUp();
                }
                else
                {
                    enemyAffected.enemy = enemyToDamage;
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
            yield return StartCoroutine(enemyAffectedInfo.enemy.PushBackAnimation(transform.position));
            if (enemyAffectedInfo.shouldKill)
            {
                enemyAffectedInfo.enemy.Kill();
            }
        }
        HasFinishedAnimation = true;
        Destroy(this.gameObject);
    }


}
