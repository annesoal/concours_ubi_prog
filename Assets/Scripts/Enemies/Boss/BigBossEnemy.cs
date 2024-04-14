using System.Collections;
using Grid;
using Grid.Blocks;
using Unity.Netcode;
using UnityEngine;

namespace Enemies.Boss
{
    public class BigBossEnemy : NetworkBehaviour
    {
        public static BigBossEnemy Instance { get; private set; }

        [SerializeField] private int nbMalus = 1;
        [SerializeField] private int ratioMovement = 8;
        [SerializeField] private GameObject malus;
        [SerializeField] protected Animator animator;
        
        private const string SPAWN_POINT_COMPONENT_ERROR =
            "Boss doit avoir le component `BlockBossSpawn`";
        
        public bool hasFinishedMoveAnimation = false;
        public  bool hasFinishedSpawnAnimation = false;


        public void SpawnMalusOnGrid()
        {
            Debug.Log("BIGBOSS SparnMalus");
            SpawnMalus.SpawnMalusOnGridPlayers(malus);
        }
        
        
        
        private void InitializeBoss()
        {
                //RunSpawnAnimation();
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
        
        private void MoveBossOnSpawnPoint(Transform spawnPoint)
        {
            bool hasComponent = spawnPoint.TryGetComponent(out BlockBossSpawn blockBossSpawn);
        
            if (hasComponent)
            {
                blockBossSpawn.SetBossOnBlock(transform);
              //  Vector3 position = this.gameObject.transform.position;
               // TilingGrid.grid.AddObjectToCellAtPosition(this.gameObject, TilingGrid.LocalToGridPosition(position));
            }
            else
            {
                Debug.LogError(SPAWN_POINT_COMPONENT_ERROR);
            }
            
        }
        
        private void Awake()
        {
            Instance = this;
        }

        
        public void SpawnMalusOnGrid(int energy)
        {
            if (!IsTimeToMove(energy)) return;
            SpawnMalus.SpawnMalusOnGridPlayers(malus);
            
        }


        private bool IsTimeToMove(int energy)
        {
            return energy % ratioMovement == 0;
        }
        
        
        
        
    }
}