using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

/// <summary>
/// "Kills" enemy on collsion.
/// </summary>
public class ZombotTrap : BaseTrap
{
    public static int SetCost; 
    public static int Damage;

    private int _cost = SetCost;
    public override int Cost { get => _cost; set => _cost = value ; }
    
    protected override void ActivateTrapBehaviour(Enemy enemy)
    {
        enemy.Damage(Damage);
    }
}
