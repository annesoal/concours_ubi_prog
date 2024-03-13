using Ennemies;
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
        
        
        // Deplacements (enum ou dict?)
        protected Vector3 _avancer = new Vector3(0, 0, -1);
        protected Vector3 _gauche = new Vector3(-1, 0, 0);
        protected Vector3 _droite = new Vector3(1, 0, 0);
        protected Vector2Int _avancer2d = new Vector2Int(0, -1);
        protected Vector2Int _gauche2d = new Vector2Int(-1, 0);
        protected Vector2Int _droite2d = new Vector2Int(1, 0);
        
        protected EnnemyGridHelper _helper;
        protected Vector2Int currentPosition2d;
        protected CellRecorder _cellRecorder; // Permet a Ennemi de verifier ses derniers mouvements
        protected Cell cell;
        
        public EnnemyType GetEnnemyType()
        {
            return ennemyType;
        }
        
        public virtual void Move()
        {
        }

        public virtual void Corrupt()
        {
        }
        
        
    }
}