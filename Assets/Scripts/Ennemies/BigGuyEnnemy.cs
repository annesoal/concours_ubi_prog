using DefaultNamespace;

namespace Ennemies
{
    public class BigGuyEnnemy : Ennemy, ICorrupt
    {
        public BigGuyEnnemy()
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