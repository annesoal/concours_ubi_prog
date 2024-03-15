using System;
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
        private GridHelper _helper;
        private Vector2Int _position2d;
        private Vector3 _position3d;
        private Recorder<Cell> recorder;
        [SerializeField] protected ObstacleType obstacleType = ObstacleType.Test;
        
        private void Start()
        {
            _position3d = transform.position;
            _helper = new PlayerSelectorGridHelper(TilingGrid.LocalToGridPosition(_position3d));
            _helper.AddOnTopCell(this.gameObject);
            
        }
        
        public ObstacleType GetObstacleType()
        {
            return obstacleType;
        }
        
        
    }
    

}