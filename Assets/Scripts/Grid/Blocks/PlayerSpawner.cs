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
            Debug.Log(_cell.position);
            Debug.Log(_cell.type);
            Vector3 position = TilingGrid.GridPositionToLocal(_cell.position);
            Debug.Log(position);
            Object.Instantiate(player, position, Quaternion.identity);
        }
    }
}