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
        public BigGuyEnemy()
        {
            ennemyType = EnnemyType.BigGuy;
        }
        
        
    }
}