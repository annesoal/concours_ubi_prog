using Enemies;

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

        public void Corrupt()
        {
           
        }
    }
}