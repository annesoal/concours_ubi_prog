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