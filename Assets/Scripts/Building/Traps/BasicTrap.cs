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

    public static int SetCost; 
    public static int StunDuration;

    private int _cost = SetCost;
    public override int Cost { get => _cost; set => _cost = value ; }


    protected override void ActivateTrapBehaviour(Enemy enemy)
    {
        enemy.SetAsStupefied(StunDuration);
    }
}
