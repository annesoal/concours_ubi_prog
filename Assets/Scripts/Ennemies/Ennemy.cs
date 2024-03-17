using Ennemies;
using Grid;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public enum EnnemyType
    {
        None = 0,
        Basic,
        BigGuy,
        Flying,
        Goofy,
        Doggo
    }

    public abstract class Ennemy : NetworkBehaviour, IDamageable
    {
        [SerializeField] protected EnnemyType ennemyType;

        [FormerlySerializedAs("RemainingMove")] [FormerlySerializedAs("speedEnnemy")] [SerializeField]
        protected int remainingMove; //Nb de blocs restants lors dun tour

        [FormerlySerializedAs("state")] [SerializeField]
        protected bool stupefiedState = false; // Piege

        [SerializeField] protected int movementPerTurn = 2;
        [SerializeField] protected int health;

        protected EnnemyGridHelper _helper;
        protected Recorder<Cell> _cellRecorder; // Permet a Ennemi de verifier ses derniers mouvements
        protected Cell cell;
        protected Vector2Int _nextPosition2d;
        protected Vector2Int currentPosition2d;
        protected Vector3 _currentPosition3d;
        protected Vector3 _nextPosition3d;


        // Deplacements 
        protected Vector3 _avancer = new Vector3(0, 0, -1);
        protected Vector3 _gauche = new Vector3(-1, 0, 0);
        protected Vector3 _droite = new Vector3(1, 0, 0);
        protected Vector2Int _avancer2d = new Vector2Int(0, -1);
        protected Vector2Int _gauche2d = new Vector2Int(-1, 0);
        protected Vector2Int _droite2d = new Vector2Int(1, 0);


        private void Initialize()
        {
        
        }
        
        public void Start()
        {
            Initialize();
        }

        public EnnemyType GetEnnemyType()
        {
            return ennemyType;
        }
        
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public void Damage()
        {
            Health--;
            if (Health < 1)
            {
                Destroy(gameObject, 0.5f);
            }
        }
        


        public virtual int GetMovementBlocPerTurn() => 2;

        /**
        * Reinitialise le nombre de mouvement d'un Ennemi
        */
        public void ResetMovement()
        {
            remainingMove = GetMovementBlocPerTurn();
        }

        public abstract void Move();

        public override void OnDestroy()
        {
        }
        
        
    }
}