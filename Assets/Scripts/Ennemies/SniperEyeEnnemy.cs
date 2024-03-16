using DefaultNamespace;

namespace Ennemies
{
    public class SniperEyeEnnemy : Ennemy, ICorrupt
    {
        
        public SniperEyeEnnemy()
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