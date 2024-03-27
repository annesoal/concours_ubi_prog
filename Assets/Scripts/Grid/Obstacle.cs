using Grid.Interface;
using UnityEngine;


namespace Grid
{
    
    public enum ObstacleType 
    {
        None = 0,
        Test
    }
    

    public class Obstacle : MonoBehaviour, ITopOfCell
    {
        public GameObject obstacle;
        [SerializeField] protected ObstacleType obstacleType = ObstacleType.Test;
        
        private void Start()
        {
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, transform.position);
        }
        public new TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Obstacle;
        }

        public GameObject ToGameObject()
        {
            return this.gameObject;
        }
    }
    

}