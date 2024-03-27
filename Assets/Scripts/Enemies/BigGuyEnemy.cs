using Enemies;
using Grid;

namespace Ennemies
{
    public class BigGuyEnemy : Enemy, ICorrupt
    {
        public BigGuyEnemy()
        {
            ennemyType = EnnemyType.BigGuy;
        }
        
        public override void Move(int energy)
        {
           
        }

        public override bool PathfindingInvalidCell(Cell cell)
        {
            throw new System.NotImplementedException();
        }

        public void Corrupt()
        {
           
        }
    }
}