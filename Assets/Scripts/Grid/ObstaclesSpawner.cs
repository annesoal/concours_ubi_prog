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
        private float valueForRandomness = 0.10f;

        public void Initialize()
        { 
            _position2d = new Vector2Int();
            _helper = new ObstacleGridHelper(_position2d); 
        }

        public void SpawnObstacles(GameObject obstacle)
        {
            for (int i = 0; i < Size; i++)
            {
                _position2d.x = i;
                for (int j = 0; j < Size; j++)
                {
                    _position2d.y = j;
                    _helper.SetHelperPosition(_position2d);
                    if (_helper.IsValidCell(_position2d) && randomBool())
                    {
                        GameObject test = SpawnObstacle(obstacle);
                        SetObjectOnTop(test, _position2d);
                    }
                }
            }
        }


        private bool randomBool()
        {
            return rand.NextDouble() > (1 - valueForRandomness);
        }
        
        private GameObject SpawnObstacle(GameObject obstacle)
        {
           // Debug.Log(_cell.position);
            Vector3 position3d = TilingGrid.GridPositionToLocal(_position2d);
            position3d.y += yPositionValue;
           // Debug.Log(_position2d);
            return Instantiate(obstacle, position3d, Quaternion.identity);
        }

        private void SetObjectOnTop(GameObject obstacle, Vector2Int position)
        {
            _cell = TilingGrid.grid.GetCell(position);
            _cell.AddGameObject(obstacle);
           // Debug.Log("REP   :" + _cell.objectsOnTop.Exists(o => o == obstacle));
        }
        
    }
}



        
    
    

