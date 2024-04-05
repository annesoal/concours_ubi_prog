using Enemies;
using Grid;

namespace Ennemies
{
    public class GoofyEnemy : BasicEnemy 
    {
        
        public GoofyEnemy()
        {
            ennemyType = EnnemyType.Goofy;
        }

        public override bool PathfindingInvalidCell(Cell cell)
        {
            throw new System.NotImplementedException();
        }
    }
}