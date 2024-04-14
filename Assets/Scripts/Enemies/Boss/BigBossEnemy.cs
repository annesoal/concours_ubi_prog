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
        
        private const string SPAWN_POINT_COMPONENT_ERROR =
            "Boss doit avoir le component `BlockBossSpawn`";
        
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Instance = this;
            }

            InitializeBoss();
        
            if (IsServer)
            {
                TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, transform.position);
               // Cell onCell = TilingGrid.
            }
        }
        
        
        private void InitializeBoss()
        {
                MoveBossOnSpawnPoint(TowerDefenseManager.Instance.BossBlockSpawn);
                
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