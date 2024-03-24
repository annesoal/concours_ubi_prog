using System.Collections.Generic;
using Ennemies;
using Grid;
using Grid.Interface;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
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

    public abstract class Enemy : NetworkBehaviour, IDamageable, ITopOfCell
    {
        [SerializeField] protected EnnemyType ennemyType;

        [SerializeField] protected bool stupefiedState = false; // Piege

        [SerializeField] protected int ratioMovement = 3;
        [SerializeField] protected int health;

        protected EnemyGridHelper _helper;
        protected Recorder<Cell> _cellRecorder; // Permet a Ennemi de verifier ses derniers mouvements
        protected Cell cell;
        protected Vector2Int _nextPosition2d;
        protected Vector2Int currentPosition2d;
        
        protected static List<GameObject> enemiesInGame = new List<GameObject>();


        // Deplacements 
        protected Vector3 _avancer = new Vector3(0, 0, -1);
        protected Vector3 _gauche = new Vector3(-1, 0, 0);
        protected Vector3 _droite = new Vector3(1, 0, 0);
        protected Vector2Int _avancer2d = new Vector2Int(0, -1);
        protected Vector2Int _gauche2d = new Vector2Int(-1, 0);
        protected Vector2Int _droite2d = new Vector2Int(1, 0);

        public float distanceFromEnd = 10.0f; 

        protected virtual void Initialize()
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

        public void Damage(int damage)
        {
            Health -= damage;
            if (Health < 1)
            {
                Die();
            }
        }
        private void Die()
        {
            enemiesInGame.Remove(gameObject);
            TilingGrid.RemoveElement(gameObject,transform.position);
            Destroy(gameObject, 0.5f);
        }
        
        public abstract void Move(int energy);
        

        public override void OnDestroy()
        {
        }

        protected void AddInGame(GameObject enemy)
        {
            enemiesInGame ??= new List<GameObject>();
            enemiesInGame.Add(enemy);
        }


        public static List<GameObject>  GetEnemiesInGame()
        {
            return enemiesInGame;
        }


        public new TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Enemy;
        }

        public GameObject ToGameObject()
        {
            return gameObject;
        }

        public static void ResetSaticData()
        {
            enemiesInGame = null;
        }
        public static List<Enemy> GetEnemiesInCells(List<Cell> cells)
        {
            List<Enemy> enemies = new List<Enemy>();
            foreach (Cell cell in cells)
            {
                if (cell.ContainsEnemy())
                {
                    Enemy enemy = cell.GetEnemy();
                    if (enemy != null)
                        enemies.Add(enemy);
                }
            }
            return enemies;
        }
    }
}