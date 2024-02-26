using UnityEngine;

namespace Grid.Blocks
{
    public class PlayerSpawner 
    {
        private Cell _cell; 
        public Cell SpawningCell
        {
            set => _cell = value;
        }
        
        public void SpawnPlayer(GameObject player)
        {
            Vector3 position = TilingGrid.GridPositionToLocal(_cell.position);
            Object.Instantiate(player, position, Quaternion.identity);
        }
    }
}