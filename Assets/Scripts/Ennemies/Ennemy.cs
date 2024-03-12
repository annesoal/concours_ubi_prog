using Grid;
using Unity.Netcode;
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
    
    public abstract class Ennemy : NetworkBehaviour
    { 
        [SerializeField] protected EnnemyType ennemyType ;
        [SerializeField] protected int speedEnnemy; //Nb de blocs pouvant avancer par tour
        [SerializeField] protected bool state = true; // Piege
        
        protected Vector2Int currentPosition2d;
        protected CellRecorder _cellRecorder; // Permet a Ennemi de verifier ses derniers mouvements
        protected Cell cell;
        
        public virtual void Move()
        {
        }

        public virtual void Corrupt()
        {
        }
        
        //Destroy
        
    }
}