using Grid;
using Grid.Blocks;
using Unity.Netcode;
using UnityEngine;

namespace Enemies.Boss
{
    public class BigBossEnemy : NetworkBehaviour
    {
        public static BigBossEnemy LocalInstance { get; private set; }
        
        [SerializeField]private SpawnMalus spawnerMalus ;
        [SerializeField] private int ratioMovement = 8;
        [SerializeField] private GameObject malus;
        
        private const string SPAWN_POINT_COMPONENT_ERROR =
            "Chaque spawn point de joueur doit avoir le component `BlockPlayerSpawn`";
        
        
        public void SpawnMalus(int energy)
        {
            if (!IsTimeToMove(energy)) return;
            spawnerMalus.SpawnMalusOnGridPlayers(malus);
            
        }


        private bool IsTimeToMove(int energy)
        {
            return energy % ratioMovement == 0;
        }
        
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                LocalInstance = this;
            }
            MoveBossOnSpawnPoint(TowerDefenseManager.Instance.BossBlockSpawn);
            
        }
        
        private void MoveBossOnSpawnPoint(Transform spawnPoint)
        {
            bool hasComponent = spawnPoint.TryGetComponent(out BlockBossSpawn blockBossSpawn);

            if (hasComponent)
            {
                blockBossSpawn.SetBossOnBlock(transform);
            }
            else
            {
                Debug.LogError(SPAWN_POINT_COMPONENT_ERROR);
            }
            
        }
        
        
    }
}