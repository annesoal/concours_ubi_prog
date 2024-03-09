using System.Collections.Generic;
using UnityEngine;

namespace Grid.Blocks
{
    public class ObjectSpawner : MonoBehaviour
    {
        private const int Size = 100;
        private Vector2Int _position;
        private System.Random rand = new System.Random();
        private GridHelper _helper;
        private GameObject _obstacle;
        public void Initialize(GameObject obstacle)
        { 
            _position = new Vector2Int();
            _helper = new ObstacleGridHelper(_position);
            _obstacle = obstacle;
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
                    if (_helper.IsValidCell(_position) && randomBool())
                    {
                        listOfPosition.Add(_position);
                    }
                    j++; 
                } while (j < Size);
                i++;
            } while (i < Size);

            return listOfPosition;
        }

        private bool randomBool()
        {
            return rand.NextDouble() > 0.8;
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
            GameObject.Instantiate(_obstacle, position3d, Quaternion.identity);
        }
        
        
    }
}



        
    
    

