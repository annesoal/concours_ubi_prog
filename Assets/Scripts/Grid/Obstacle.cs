using System;
using Grid.Interface;
using Unity.Netcode;
using UnityEngine;


namespace Grid
{
    
    public enum ObstacleType 
    {
        None = 0,
        Test
    }
    

    public class Obstacle : NetworkBehaviour, ITopOfCell
    {
        [SerializeField] private float topOfCell = 0.72f;
       
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
        
        //TODO tester
        private void OnDestroy()
        {
            TilingGrid.RemoveElement(gameObject, TilingGrid.LocalToGridPosition(transform.position));
        }
    }
    

}