using UnityEngine;

namespace Grid.Blocks
{
    public class PlayerSpawner
    {
        private Vector3 _position; 
        public PlayerSpawner(Vector3 position)
        {
            _position = position;
        }
        public void SpawnPlayer(GameObject player)
        {
            Object.Instantiate(player, _position, Quaternion.identity);
        }
    }
}