using System;
using System.Collections.Generic;
using System.Linq;
using Building.Towers;
using Enemies;
using Enemies.Basic;
using Grid;
using Grid.Interface;
using Unity.VisualScripting;
using UnityEngine;


/**
 * Tour tirant les ennemis les plus loins d'elle sur un axe particulier.
 */
public class BasicTower : BaseTower
{
    public static int BasicTowerRange; 
    public static int BasicTowerDamage; 
    public static int BasicTowerTimeBetweenShots; 
    public static int BasicTowerHealth; 
    public static int BasicTowerProjectilesNumber;
    public static int BasicTowerCost;

    private int _range = BasicTowerRange;
    private int _damage = BasicTowerDamage;
    private int _timeBetweenShots = BasicTowerTimeBetweenShots;
    private int _health = BasicTowerHealth;
    private int _projectileNumber = BasicTowerProjectilesNumber;
    private int _cost = BasicTowerCost;
    
    
    public override int Cost { get => _cost; set => _cost = value ; }
    public override int AttackDamage { get => _damage; set => _damage = value; }
    public override int Health { get => _health; set => _health = value; }
    public override int TimeBetweenShots { get => _timeBetweenShots; set => _timeBetweenShots = value; }
    public override int Range { get => _range; set => _range = value; }
    public override int TotalOfProjectile { get => _projectileNumber; set => _projectileNumber = value; }

    protected override List<EnemyInfoToShoot> TargetEnemies()
    {
        List<Cell> cellsInShootingRange = GetCellsInShootingRange();
        List<EnemyInfoToShoot> infos = new List<EnemyInfoToShoot>(); 
        for (int i = 0; i < TotalOfProjectile; i++)
        {
            Enemy enemy = TargetFarthestEnemy(cellsInShootingRange);
            if (!enemy)
            {
                break;
            }
            int remainingHP = enemy.Damage(AttackDamage);
            if (remainingHP <= 0)
            {
                EnemyInfoToShoot enemyInfoToShoot = new EnemyInfoToShoot()
                {
                    shouldKill = true,
                    enemy = enemy,
                    position = enemy.ToGameObject().transform.position,
                };
                infos.Add(enemyInfoToShoot);
                enemy.CleanUp();
            }
            else
            {
                EnemyInfoToShoot enemyInfoToShoot = new EnemyInfoToShoot()
                {
                    shouldKill = false,
                    enemy = enemy,
                    position = enemy.ToGameObject().transform.position,
                };
                infos.Add(enemyInfoToShoot);
            }
            
        }


        return infos;
    }

    private List<Cell> GetCellsInShootingRange()
    {
        Vector2Int origin = TilingGrid.LocalToGridPosition(transform.position);
        List<Cell> cellsInShootingRange = TilingGrid.grid.GetCellsInRadius(origin, Range);
        return cellsInShootingRange;
    }


    private const int INITIAL_NUMBER_OF_TARGET_SET = 0;
    private const int NO_ENEMY_FOUND_IN_LIST = -1;
    
    private Enemy TargetFarthestEnemy(List<Cell> cellsInShootingRange)
    {
        float distanceToJimmy = 0.0f;
        Enemy toTarget = null;
        foreach (var cell in cellsInShootingRange)
        {
            Enemy enemy = cell.GetEnemy();
            if (enemy)
            {
                float distance = enemy.DistanceToDestination();
                if (distance > distanceToJimmy)
                {
                    toTarget = enemy;
                }
            }
        }

        return toTarget;
    }
    
    public override bool IsWalkable()
    {
        return false;
    }
}
