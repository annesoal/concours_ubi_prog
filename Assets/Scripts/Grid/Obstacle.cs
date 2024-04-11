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
        [SerializeField] protected Animator animator;
        [SerializeField] private float topOfCell = 0.72f;

        [SerializeField] private int health = 1;
        [SerializeField] protected ObstacleType obstacleType = ObstacleType.Test;

        public void Initialize()
        {
            if (!IsServer) return;
            Vector2Int vector2Int = TilingGrid.LocalToGridPosition(this.transform.position);
            TilingGrid.grid.AddObjectToCellAtPositionInit(this.gameObject, vector2Int);
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
            if (Health < 1 && IsServer)
            {
                TilingGrid.RemoveElement(gameObject, transform.position); 
                DestroyClientRpc();
            }
        }

        [ClientRpc]
        private void DestroyClientRpc()
        {
            Destroy(this.gameObject);
        }
    }
}