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
        Big,
        Flying,
        Fast
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
        protected CellRecorder _cellRecorder; // Permet a Ennemi de verifier ses derniers mouvements

        protected Cell cell;
        protected Vector2Int _nextPosition2d;
        protected Vector2Int currentPosition2d;
        protected Vector3 _currentPosition3d;
        protected Vector3 _nextPosition3d;


        // Deplacements (enum?)
        protected Vector3 _avancer = new Vector3(0, 0, -1);
        protected Vector3 _gauche = new Vector3(-1, 0, 0);
        protected Vector3 _droite = new Vector3(1, 0, 0);
        protected Vector2Int _avancer2d = new Vector2Int(0, -1);
        protected Vector2Int _gauche2d = new Vector2Int(-1, 0);
        protected Vector2Int _droite2d = new Vector2Int(1, 0);


        protected void Initialize()
        {
            remainingMove = GetMovementBlocPerTurn();
            cell = new Cell();
            _nextPosition2d = new Vector2Int();
            _nextPosition3d = new Vector3();
            _currentPosition3d = transform.position;
            currentPosition2d = TilingGrid.LocalToGridPosition(_currentPosition3d);
            _cellRecorder = new CellRecorder();
            _helper = new EnnemyGridHelper(currentPosition2d, _cellRecorder);
            _helper.AddOnTopCell(this.gameObject);
        }

        public EnnemyType GetEnnemyType()
        {
            return ennemyType;
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

        public abstract void Corrupt();

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
    }
}