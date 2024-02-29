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
    
    public abstract class Ennemy : MonoBehaviour
    {
        protected EnnemyType ennemyType;
        
        public Ennemy()
        {
            ennemyType = EnnemyType.None;
        }

        
        public virtual void Move()
        {
        }

        public virtual void Corrupt()
        {
        }
        
        //Destroy
        
    }
}