using Enemies;
using Grid;

namespace Ennemies
{
    public class SniperEyeEnemy : Enemy, ICorrupt
    {
        
        public SniperEyeEnemy()
        {
            ennemyType = EnnemyType.Flying;
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