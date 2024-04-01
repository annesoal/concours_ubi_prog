using System;
using System.Collections.Generic;
using Ennemies;
using Grid;
using Grid.Interface;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
using Interfaces;

namespace Enemies
{
    public class BigGuyEnemy : AttackingEnemy
    {
        [SerializeField] private int _enemyDomage = 1;
        [SerializeField] private int _attackRate;
        [SerializeField] private int radiusAttack = 1;
        
        public BigGuyEnemy()
        {
            ennemyType = EnnemyType.BigGuy;
        }
        
        
    }
}