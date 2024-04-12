using System;
using System.Collections;
using System.Collections.Generic;
using Ennemies;
using Grid;
using Grid.Interface;
using Unity.Netcode;
using UnityEngine;
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
        public static int Energy;
        
        protected EnnemyType ennemyType;

        public abstract int MoveRatio { get; set; }

        [SerializeField] protected int isStupefiedState = 0; // Piege

        private List<Cell> _destinationsCell;
        protected int timeSinceLastAction = 0;

        protected Cell cell;
        public bool hasPath = false;
        public List<Cell> path;
        public static List<GameObject> enemiesInGame = new List<GameObject>();

        public bool hasFinishedToMove = false;
        public bool hasFinishedMoveAnimation = false;
        public  bool hasFinishedSpawnAnimation = false; 
        
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
            TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        
            Initialize();
            SetDestinations();
            RunSpawnAnimation();
        }

        private void RunSpawnAnimation()
        {
            StartCoroutine(AnimationSpawn());
        }

        private IEnumerator AnimationSpawn()
        {
            float timeToAnimate = 0.3f;
            float currentTime = 0.0f;
            hasFinishedSpawnAnimation = false;
            animator.SetBool("Spawn", true);
            while (currentTime < timeToAnimate)
            {
                yield return null;
                currentTime += Time.deltaTime;
            }
            animator.SetBool("Spawn", false);
            hasFinishedSpawnAnimation = true;
        }

         protected bool AnimationSpawnIsFinished()
        {
            return hasFinishedSpawnAnimation;
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


        public abstract int Health { get; set; }

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
        }

        public static event EventHandler OnAnyEnemyMoved;
        //public abstract IEnumerator Move();

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

        public void SetAsStupefied(int stunDuration)
        {
            isStupefiedState = stunDuration;
        }
        
        public void ResetStupefiedState()
        {
            isStupefiedState = 0;
        }
        
        private void TowerDefenseManager_OnCurrentStateChanged
            (object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
        {
            if (e.newValue != TowerDefenseManager.State.EnvironmentTurn)
            {
                isStupefiedState--;
            }
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

        public void ResetAnimationStates()
        {
            hasFinishedToMove = false; 
            hasFinishedMoveAnimation = false; 
        }

        protected abstract (bool moved, bool attacked, Vector3 destination) BackendMove();
        
        public EnemyChoicesInfo CalculateChoices()
        {
            EnemyChoicesInfo infos = new EnemyChoicesInfo();
            infos.origin = this.gameObject.transform.position;
            (bool moved, bool attacked, Vector3 destination) recordedResult = BackendMove();

            infos.destination = recordedResult.destination;
            infos.hasMoved = recordedResult.moved;
            infos.hasAttacked = recordedResult.attacked;
            
            
            throw new NotImplementedException();
            return new EnemyChoicesInfo();
        }

        public IEnumerator MoveCorroutine(EnemyChoicesInfo infos)
        {
            throw new NotImplementedException();
           yield break;
        }
    }
}
