using System;
using System.Collections.Generic;
using Ennemies;
using Grid;
using Grid.Interface;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Type = Grid.Type;

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

        [SerializeField] protected int ratioMovement = 1;
        [SerializeField] protected int health;
        private List<Cell> _destinationsCell;

        protected Cell cell;
        public bool hasPath = false;
        public List<Cell> path;
        
        public static List<GameObject> enemiesInGame = new List<GameObject>();


        // Deplacements 
        protected Vector2Int _gauche2d = new Vector2Int(-1, 0);
        protected Vector2Int _droite2d = new Vector2Int(1, 0);

        protected virtual void Initialize()
        {
            
        }
        
        public void Start()
        {
            Initialize();
            SetDestinations();
        }

        private void SetDestinations()
        {
            _destinationsCell = TilingGrid.grid.GetCellsOfType(Type.EnemyDestination);
        }

        private Cell GetClosestDestination()
        {
            if (_destinationsCell == null || _destinationsCell.Count == 0)
                throw new Exception("Destination cells are not set or were not found !");

            Cell destinationToReturn = _destinationsCell[0];
            float destinationDistance = Cell.Distance(cell, destinationToReturn);
            for (int i = 1; i < _destinationsCell.Count; i++)
            {
                Cell currentCell = _destinationsCell[i];
                float currentCellDistance = Cell.Distance(currentCell, cell);
                if (currentCellDistance < destinationDistance)
                {
                    destinationDistance = currentCellDistance;
                    destinationToReturn = currentCell;
                }
            }
            return destinationToReturn;
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

        protected void Die()
        {
            Debug.Log("Should Die");
            enemiesInGame.Remove(this.gameObject);
            TilingGrid.RemoveElement(gameObject,transform.position);
            Destroy(this.gameObject);
        }

        public static event EventHandler OnAnyEnemyMoved;
        public abstract void Move(int energy);

        protected void EmitOnAnyEnemyMoved()
        {
            OnAnyEnemyMoved?.Invoke(this, EventArgs.Empty);
        }
        

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
            OnAnyEnemyMoved = null;
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

        public abstract bool PathfindingInvalidCell(Cell cell);

        public Cell GetCurrentPosition()
        {
            Vector2Int positionOnGrid = TilingGrid.LocalToGridPosition(gameObject.transform.position);
            cell = TilingGrid.grid.GetCell(positionOnGrid);
            return cell;
        }

        public Cell GetDestination()
        {
            return GetClosestDestination();
        }
    }
}