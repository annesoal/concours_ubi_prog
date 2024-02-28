using UnityEngine;

namespace Grid.Blocks
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        private const int Size = 100;
        private Cell _cell = new Cell();
        private Vector2Int _position;
        private System.Random rand = new System.Random();
        private GridHelper _helper;

        public void Initialize()
        { 
            _position = new Vector2Int();
            _helper = new ObstacleGridHelper(_position); 
        }

        public void SpawnObstacles(GameObject obstacle)
        {
            int i = 0;
            do
            {
                _position.x = i;
                int j = 0;
                do
                {
                    _position.y = j; 
                    _helper.SetHelperPosition(_position);
                    Debug.Log(_helper.GetHelperPosition());
                    //Debug.Log(_helper == null);
                    if (_helper.IsValidCell(_position) && randomBool())
                    {
                        Debug.Log(_position);
                        SpawnObstacle(obstacle);
                    }
                    j++; 
                } while (j < Size);
                i++;
            } while (i < Size);
        }

        private bool randomBool()
        {
            return rand.NextDouble() > 0.8;
        }
        
        private void SpawnObstacle(GameObject obstacle)
        {
            Debug.Log(_cell.position);
            
            Vector3 position3d = TilingGrid.GridPositionToLocal(_position);
            position3d.y += 0.5f;
            Debug.Log(_position);
            Instantiate(obstacle, position3d, Quaternion.identity);
        }

        
    }
}



        
    
    

