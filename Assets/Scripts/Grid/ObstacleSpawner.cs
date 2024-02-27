using System;
using UnityEngine;
using static Unity.Mathematics.Random;
using Object = UnityEngine.Object;


namespace Grid.Blocks
{
    public class ObstacleSpawner : MonoBehaviour
    {
        private ObstacleGridHelper _helper;
        private const int Size = 100;
        private Cell _cell = new Cell();
        private Vector2Int _position = new Vector2Int();
        private System.Random rand = new System.Random();
        
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




        
    
    

