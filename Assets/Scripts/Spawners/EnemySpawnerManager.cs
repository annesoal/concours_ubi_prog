using System;
using System.Collections.Generic;
using Grid;
using Grid.Blocks;
using Unity.Netcode;
using Type = Grid.Type;

namespace Managers
{
    public class EnemySpawnerManager : NetworkBehaviour
    {
        private List<SpawnerBlock> spawners;

        private void Start()
        {
            if (IsServer)
            {
                TilingGrid.grid.GetCellsOfType(Type.EnemySpawnBlock);

            }
        }
    }
}