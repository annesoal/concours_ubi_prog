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
        protected float timeToDie = 0.2f;

        public abstract int MoveRatio { get; set; }
        
        [SerializeField] protected int isStupefiedState = 0; // Piege

        private List<Cell> _destinationsCell;
        protected int _actionTimer = 0;

        protected Cell cell;
        public bool hasPath = false;
        public List<Cell> path;
        public static List<GameObject> enemiesInGame = new List<GameObject>();

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

        public int Damage(int damage)
        {
             return Health -= damage;
        }

        protected void Die()
        {
            enemiesInGame.Remove(this.gameObject);
            animator.SetBool("Die", true);
            TilingGrid.RemoveElement(this.gameObject, transform.position);
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

        public static List<GameObject> GetEnemiesInGameCopy()
        {
            return new List<GameObject>(enemiesInGame);
        }
        

        public new TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Enemy;
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
        

        
        public GameObject ToGameObject()
        {
            return gameObject;
        }

        public static void ResetSaticData()
        {
            enemiesInGame = new List<GameObject>();
        }

        public void ResetAnimationStates()
        {
            hasFinishedMoveAnimation = false; 
        }
       
        protected abstract IEnumerator RotateThenMove(Vector3 destination);
        protected abstract EnemyChoicesInfo BackendMove();
        
        public EnemyChoicesInfo CalculateChoices()
        {
            return BackendMove();
        }

        private void FinishingMoveAnimation()
        {
            Debug.LogWarning("Finishing move");
            animator.SetBool("Die", true); 
            StartCoroutine(Dying());

        }

        private IEnumerator Dying()
        {
            var timeNow = 0.0f;
            while (timeNow < timeToDie)
            {
                timeNow += Time.deltaTime;
                yield return null;
            }

            if (IsServer)
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        public virtual void MoveCorroutine(EnemyChoicesInfo infos)
        {
            hasFinishedMoveAnimation = false;
            if (infos.hasReachedEnd)
            {
                hasFinishedMoveAnimation = true;
                FinishingMoveAnimation();
                return;
            }
            if (infos.hasMoved == false)
            {
                hasFinishedMoveAnimation = true;
                return;
            }

            StartCoroutine(RotateThenMove(infos.destination));
        }

        public IEnumerator PushBackAnimation(Vector3 pushedFrom)
        {
            Vector3 origin = this.gameObject.transform.position;
            Vector3 directionToGo = origin - pushedFrom;
            float intensity = 0.1f;
            float timeNow = 0.0f;
            float timeToPush = 0.15f;
            while (timeNow < timeToPush)
            {
                this.transform.position = Vector3.Lerp(origin, directionToGo + origin, intensity * timeNow/timeToPush);
                yield return null;
                
                timeNow += Time.deltaTime;
            }
            
            timeNow = 0;
            Vector3 newPos = this.gameObject.transform.position;
            while (timeNow < timeToPush)
            {
                this.transform.position = Vector3.Lerp(newPos,origin, timeNow/timeToPush);
                yield return null;
                timeNow += Time.deltaTime;
            }
        }

        public void CleanUp()
        {
            enemiesInGame.Remove(this.gameObject);
            TilingGrid.grid.RemoveObjectFromCurrentCell(this.gameObject);
        }
        public void Kill()
        {
            StartCoroutine(Dying());
        }

        public float DistanceToDestination()
        {
            Cell destination = GetDestination();
            Vector3 destination3D = TilingGrid.GridPositionToLocal(destination.position);
            return Vector3.Distance(this.transform.position, destination3D);
        }
    }
}
