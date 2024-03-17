using DefaultNamespace;

namespace Ennemies
{
    public class SniperEyeEnemy : Enemy, ICorrupt
    {
        
        public SniperEyeEnemy()
        {
            ennemyType = EnnemyType.Flying;
        }
        public override void Move()
        {
           
        }
        
        public void Corrupt()
        {
           
        }
    }
}