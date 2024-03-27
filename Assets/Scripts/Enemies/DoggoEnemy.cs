using Enemies;
using Grid;

namespace Ennemies
{
    public class DoggoEnemy : Enemy
    {
        
        public DoggoEnemy()
        {
            ennemyType = EnnemyType.Doggo;
        }
        public override void Move(int energy)
        {
         
        }

        public override bool PathfindingInvalidCell(Cell cell)
        {
            throw new System.NotImplementedException();
        }
    }
}