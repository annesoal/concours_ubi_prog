using UnityEngine;

namespace Grid.Blocks
{
    public class EnnemySpawner
    {
        private Vector3 _position;

        public EnnemySpawner(Vector3 position)
        {
            _position = position;
        }
        
        public void SpawnEnnemy(GameObject ennemy)
        {
            Object.Instantiate(ennemy, _position, Quaternion.identity);
        }
    }
}