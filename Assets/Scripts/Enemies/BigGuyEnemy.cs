using DefaultNamespace;

namespace Ennemies
{
    public class BigGuyEnemy : Enemy, ICorrupt
    {
        public BigGuyEnemy()
        {
            ennemyType = EnnemyType.BigGuy;
        }
        
        public override void Move()
        {
           
        }

        public void Corrupt()
        {
           
        }
    }
}