using Enemies;
using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;

namespace Ennemies
{
    public class GoofyEnemy : BasicEnemy
    {
        public GoofyEnemy()
        {
            ennemyType = EnnemyType.Goofy;
        }


    }
}