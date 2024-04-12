using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;

namespace Enemies.Basic
{
    public class GoofyEnemy : BasicEnemy 
    {
        public static int GoofyHealth;
        public static int GoofyMoveRation;
        
        private int _health = GoofyHealth;
        private int _moveRatio = GoofyMoveRation; 
        
        public override int MoveRatio { get => _moveRatio; set => _moveRatio = value; }

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
