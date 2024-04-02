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

        public SniperEyeEnemy()
        {
            ennemyType = EnnemyType.Flying;
        }
    }
}