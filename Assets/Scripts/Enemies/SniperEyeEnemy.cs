using Enemies;

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
        
        public void Corrupt()
        {
           
        }
    }
}