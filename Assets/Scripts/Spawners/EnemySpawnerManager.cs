using System.Collections.Generic;
using Enemies;
using Grid;
using Grid.Blocks;
using Unity.Netcode;
using UnityEngine;
using Type = Grid.Type;

namespace Managers
{
    public class EnemySpawnerManager : NetworkBehaviour
    {
        public static EnemySpawnerManager Instance {private set; get; }
        private List<SpawnerBlock> _spawners;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            if (IsServer)
            {
                
            }
        }

        public void SetSpawners(List<SpawnerBlock> spawners)
        {
            this._spawners = spawners;
        }

        public void StartMathSpawners(int turn)
        {
            foreach (var spawner in _spawners)
            {
               // spawner.CalculateSpawnRate(turn);
            } 
        }
        public void Spawn(int turn)
        {
            if (turn <= 0) return;
            foreach (var spawner in _spawners)
            {
                GameObject enemyToSpawn = spawner.GetEnemyToSpawn();
                if (enemyToSpawn == null)
                    continue;
                GameObject enemySpawned = Instantiate(enemyToSpawn, spawner.positionToSpawn);
                TilingGrid.grid.PlaceObjectAtPositionOnGrid(enemySpawned.gameObject, spawner.positionToSpawn.position);
                enemySpawned.GetComponent<NetworkObject>().Spawn(true);
            }
        }
    }
}