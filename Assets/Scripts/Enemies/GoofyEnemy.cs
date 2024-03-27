using Enemies;
using Grid;

namespace Ennemies
{
    public class GoofyEnemy : Enemy
    {
        
        public GoofyEnemy()
        {
            ennemyType = EnnemyType.Goofy;
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