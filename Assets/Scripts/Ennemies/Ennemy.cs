using UnityEngine;

namespace DefaultNamespace
{

    public enum EnnemyType
    {
        None = 0,
        Basic,
        Big,
        Flying,
        Fast
    }
    
    public class Ennemy : MonoBehaviour, IMove, ICorrupt
    {
        protected EnnemyType ennemyType;

        public Ennemy()
        {
            ennemyType = EnnemyType.None;
        }

        
        public virtual void Move()
        {
            throw new System.NotImplementedException();
        }

        public virtual void CorruptDream()
        {
            throw new System.NotImplementedException();
        }

        public virtual void CorruptBuilding()
        {
            throw new System.NotImplementedException();
        }
    }
}