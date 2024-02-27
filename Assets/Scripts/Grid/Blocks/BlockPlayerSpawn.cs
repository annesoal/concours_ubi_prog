using UnityEngine;

namespace Grid.Blocks
{
    public class BlockPlayerSpawn : BasicBlock 
    {
        private PlayerSpawner _spawner;

        void Awake()
        {
            Vector3 position = new(
                           transform.position.x,
                           TilingGrid.TopOfCell, 
                           transform.position.z); 
            _spawner = new(position); 
        }

        public void SpawnPlayer(GameObject player)
        {
           _spawner.SpawnPlayer(player); 
        }
        
    }
}