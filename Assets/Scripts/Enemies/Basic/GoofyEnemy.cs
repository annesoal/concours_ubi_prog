using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;

namespace Enemies.Basic
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
