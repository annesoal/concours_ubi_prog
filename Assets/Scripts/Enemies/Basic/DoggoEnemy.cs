using Random = System.Random;

namespace Enemies.Basic
{
    public class DoggoEnemy : BasicEnemy
    {
        
        public DoggoEnemy()
        {
            ennemyType = EnnemyType.Doggo;
        }


        public override int MoveRatio { get; set; }
    }
}