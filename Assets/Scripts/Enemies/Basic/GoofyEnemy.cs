using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;

namespace Enemies.Basic
{
    public class GoofyEnemy : BasicEnemy 
    {
        public static int GoofyHealth;
        private int _health = GoofyHealth;

        public override int Health
        {
            get => _health;
            set => _health = value;
        }
        public GoofyEnemy()
        {
            ennemyType = EnnemyType.Goofy;
        }
    }
}
