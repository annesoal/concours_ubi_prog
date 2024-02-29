using UnityEngine;


namespace Grid
{
    
    public enum ObstacleType 
    {
        None = 0,
        Test
    }
    public class Obstacle : MonoBehaviour
    {
        public GameObject obstacle;
        [SerializeField] protected ObstacleType obstacleType = ObstacleType.Test;
     
        
        public ObstacleType GetObstacleType()
        {
            return obstacleType;
        }
        
        
        
    }
    

}