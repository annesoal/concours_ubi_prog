using System;
using System.Collections;
using System.Collections.Generic;
using Ennemies;
using Grid;
using Grid.Interface;
using Interfaces;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Type = Grid.Type;

namespace Enemies
{
    public enum EnnemyType
    {
        None = 0,
        PetiteMerde,
        BigGuy,
        Flying,
        Goofy,
        Doggo
    }

    public abstract class Enemy : NetworkBehaviour, IDamageable, ITopOfCell //, ICanDamage
    {
        [SerializeField] protected EnnemyType ennemyType;

        [SerializeField] protected bool stupefiedState = false; // Piege

        [SerializeField] protected int ratioMovement = 1;
        private List<Cell> _destinationsCell;

        protected Cell cell;
        public bool hasPath = false;
        public List<Cell> path;
        public static List<GameObject> enemiesInGame = new List<GameObject>();

        protected bool hasFinishedToMove = false;
        protected bool hasFinishedMoveAnimation = false; 
        
        // Deplacements 
        protected Vector2Int _gauche2d = new Vector2Int(-1, 0);
        protected Vector2Int _droite2d = new Vector2Int(1, 0);
        
        [SerializeField] protected Animator animator;
        protected void Initialize()
        {
            AddInGame(this.gameObject);
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


        protected abstract bool TryStepBackward();
        
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


        public int Health
        {
            get { return _health; }
            set { _health = value; }
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
            animator.SetBool("Die", true);
            TilingGrid.RemoveElement(this.gameObject, transform.position);
            Destroy(this.gameObject);
        }



        public static event EventHandler OnAnyEnemyMoved;
        public abstract IEnumerator Move(int energy);

        public bool hasFinishedMoving()
        {
            return hasFinishedToMove;
        }


        protected void AddInGame(GameObject enemy)
        {
            enemiesInGame ??= new List<GameObject>();
            enemiesInGame.Add(enemy);
        }


        public void RemoveInGame()
        {
            enemiesInGame.Remove(this.gameObject);
        }

        public static List<GameObject> GetEnemiesInGame()
        {

            return enemiesInGame;
        }
        

        public new TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Enemy;
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

        public static int baseHealth;
        private int _health = baseHealth;
        public static int baseAttack;
        private int _attackDamage = baseAttack;

        public int AttackDamage
        {
            get => _attackDamage;
            set => value = _attackDamage;
        }
        
        protected void EmitOnAnyEnemyMoved()
        {
            OnAnyEnemyMoved?.Invoke(this, EventArgs.Empty);
        }
        
        public GameObject ToGameObject()
        {
            return gameObject;
        }

        public static void ResetSaticData()
        {
            enemiesInGame = new List<GameObject>();
            OnAnyEnemyMoved = null;
        }
    }
}