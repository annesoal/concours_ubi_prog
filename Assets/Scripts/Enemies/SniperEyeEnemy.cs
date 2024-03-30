using Enemies;
using Grid;
using Grid.Interface;

namespace Ennemies
{
    public class SniperEyeEnemy : Enemy, ICorrupt<Cell>
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

        public void Corrupt(Cell cell)
        {
           
        }
    }
}