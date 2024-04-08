using Enemies;
using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;

namespace Ennemies
{
    public class GoofyEnemy : BasicEnemy 
    {
        private Random _rand = new();
        public GoofyEnemy()
        {
            ennemyType = EnnemyType.Goofy;
        }
    }
}
