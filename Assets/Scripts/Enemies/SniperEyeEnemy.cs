using System.Collections.Generic;
using Ennemies;
using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;
namespace Enemies
{
    public class SniperEyeEnemy : AttackingEnemy
    {
     
        [SerializeField] private int _enemyDomage = 1;
        [SerializeField] private int _attackRate;
        [SerializeField] private int radiusAttack = 1;
        
        
        public SniperEyeEnemy()
        {
            ennemyType = EnnemyType.Flying;
        }
    }
}