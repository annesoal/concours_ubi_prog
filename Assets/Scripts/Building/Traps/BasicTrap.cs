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
    
    protected override void ActivateTrapBehaviour(Enemy enemy)
    {
        enemy.SetAsStupefied();
    }
}
