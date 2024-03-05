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
        [SerializeField]
        protected EnnemyType ennemyType ;

        [SerializeField] 
        protected int speedEnnemy; //Tnb de bloc pouvant avancer par tour

        [SerializeField] 
        protected bool state = true; // peut avancer ou est piege
        
        protected Vector2Int currentPosition2d;
        
        
        public virtual void Move()
        {
        }

        public virtual void Corrupt()
        {
        }
        
        //Destroy
        
    }
}