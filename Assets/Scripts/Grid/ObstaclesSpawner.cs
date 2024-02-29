using UnityEngine;

namespace Grid.Blocks
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        private const int Size = 100;
        private Cell _cell = new Cell();
        private Vector2Int _position2d;
        private System.Random rand = new System.Random();
        private GridHelper _helper;
        private float yPositionValue = 0.5f;
        
        [SerializeField]
        private float valueForRandomness = 0.15f;

        public void Initialize()
        { 
            _position2d = new Vector2Int();
            _helper = new ObstacleGridHelper(_position2d); 
        }

        public void SpawnObstacles(GameObject obstacle)
        {
            int i = 0;
            do
            {
                _position2d.x = i;
                int j = 0;
                do
                {
                    _position2d.y = j; 
                    _helper.SetHelperPosition(_position2d);
                    // Debug.Log(_helper.GetHelperPosition());
                    //Debug.Log(_helper == null);
                    if (_helper.IsValidCell(_position2d) && randomBool())
                    {
                       // Debug.Log(_position2d);
                        GameObject test = SpawnObstacle(obstacle);
                        tellPresenceAtCell(test, _position2d);
                    }
                    j++; 
                } while (j < Size);
                i++;
            } while (i < Size);
        }

        private bool randomBool()
        {
            return rand.NextDouble() > (1 - valueForRandomness);
        }
        
        private GameObject SpawnObstacle(GameObject obstacle)
        {
            Debug.Log(_cell.position);
            Vector3 position3d = TilingGrid.GridPositionToLocal(_position2d);
            position3d.y += yPositionValue;
            Debug.Log(_position2d);
            return Instantiate(obstacle, position3d, Quaternion.identity);
        }

        private void tellPresenceAtCell(GameObject obstacle, Vector2Int position)
        {
            _cell = TilingGrid.grid.GetCell(position);
            _cell.AddGameObject(obstacle);
            Debug.Log("REP   :" + _cell.objectsOnTop.Exists(o => o == obstacle));
        }
        
    }
}



        
    
    

