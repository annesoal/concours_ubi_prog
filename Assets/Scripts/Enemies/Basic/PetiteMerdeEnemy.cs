namespace Enemies.Basic
{
    public class PetiteMerdeEnemy : BasicEnemy
    {
        public static int MerdeHealth;
        public static int MerdeMoveRatio;
        
        private int _health = MerdeHealth;
        private int _moveRatio = MerdeMoveRatio;

        public override int MoveRatio { get => _moveRatio; set => _moveRatio = value; }
        

        public override int Health
        {
            get => _health;
            set => _health = value;
        }


        public PetiteMerdeEnemy()
        {
            ennemyType = EnnemyType.PetiteMerde;
        }
    }
}