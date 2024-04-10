using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Grid;
using UnityEngine;

/// <summary>
/// Stupefies enemy on collision.
/// </summary>
public class BasicTrap : BaseTrap
{
    /// Range of "explosion" in up,down,right,left direction
    [SerializeField] private int crossRange;

    private List<Enemy> _stupefiedEnemies;
    
    private void Awake()
    {
        _stupefiedEnemies = new List<Enemy>();
    }

    protected override void ActivateTrapBehaviour(Enemy enemy)
    {
        enemy.SetAsStupefied();
        _stupefiedEnemies.Add(enemy);
    }
}
