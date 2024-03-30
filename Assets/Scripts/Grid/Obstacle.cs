using System;
using Ennemies;
using Grid.Interface;
using Unity.Netcode;
using UnityEngine;
using Interfaces;

namespace Grid
{
    public enum ObstacleType
    {
        None = 0,
        Test
    }


    public class Obstacle : NetworkBehaviour, ITopOfCell, IDamageable
    {
        [SerializeField] private float topOfCell = 0.72f;

        [SerializeField] private int health = 1;
        [SerializeField] protected ObstacleType obstacleType = ObstacleType.Test;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(this.gameObject, transform.position);
        }
        public new TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Obstacle;
        }

        public GameObject ToGameObject()
        {
            return this.gameObject;
        }

        
        public int Health
        {
            get { return health; }
            set => health = value;
        }

        public void Damage(int damage)
        {
            Health -= damage;
            if (Health < 1)
            {
                TilingGrid.RemoveElement(gameObject, transform.position);
                Destroy(this.gameObject);
            }
        }
    }
}