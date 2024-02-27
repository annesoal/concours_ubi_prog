using UnityEngine;

namespace Grid.Blocks
{
    public class ObstacleSpawner : MonoBehaviour
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

        public void SpawnObstacle(GameObject obstacle)
        {
            int i = 0;
            int j = 0;
           
            do
            {
                _position.x = i;
                _position.y = j; 
                do
                {
                    Debug.Log(_helper == null);
                    if (_helper.IsValidCell(_position) && randomBool())
                    { 
                        Debug.Log(_cell.position);
                        Debug.Log(_cell.type);
                        Vector3 position3d = TilingGrid.GridPositionToLocal(_position);
                        position3d.y += 1; // TODO Je pense bien que ca mettre l'obstacle dessus
                        Debug.Log(_position);
                        Instantiate(obstacle, position3d, Quaternion.identity);
                    }
                    j++; 
                } while (j < Size);
                i++;
            } while (i < Size);
        }

        private bool randomBool()
        {
       
            return rand.NextDouble() > 0.5;
        }
    }
}




        
    
    

