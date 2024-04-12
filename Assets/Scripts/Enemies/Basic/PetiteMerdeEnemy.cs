namespace Enemies.Basic
{
    public class PetiteMerdeEnemy : BasicEnemy
    {
        public static int MerdeHealth;
        private int _health = MerdeHealth;

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