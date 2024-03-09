using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class ObjectSpawner
    {
        private Vector2Int _position;
        private System.Random _rand = new System.Random();
        private GridHelper _helper;
        private GameObject _objectToSpawn;
        private double _spawnRate;
        public void Initialize(GameObject objectToSpawn, double spawnRate = 0.8)
        { 
            _position = new Vector2Int();
            _helper = new ObstacleGridHelper(_position);
            _objectToSpawn = objectToSpawn;
            _spawnRate = spawnRate;
        }
        public List<Vector2Int> GeneratePositions()
        {
            List<Vector2Int> listOfPosition = new();
            int i = 0;
            do
            {
                _position.x = i;
                int j = 0;
                do
                {
                    _position.y = j; 
                    _helper.SetHelperPosition(_position);
                    if (_helper.IsValidCell(_position) && RandomBool())
                    {
                        listOfPosition.Add(_position);
                    }
                    j++; 
                } while (j < TilingGrid.Size);
                i++;
            } while (i < TilingGrid.Size);

            return listOfPosition;
        }

        private bool RandomBool()
        {
            return _rand.NextDouble() > _spawnRate;
        }

        public void InstantiateObstacles(List<Vector2Int> listOfPosition)
        {
            foreach (Vector2Int position in listOfPosition)
            {
                InstantiateObstacle(position);
            }
        }
        private void InstantiateObstacle(Vector2Int position)
        {
            Vector3 position3d = TilingGrid.GridPositionToLocal(position);
            position3d.y += 0.5f;
            Object.Instantiate(_objectToSpawn, position3d, Quaternion.identity);
        }
        
        
    }
}