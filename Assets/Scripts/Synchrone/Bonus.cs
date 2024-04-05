using System;
using Grid;
using Grid.Interface;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Synchrone
{
    public class Bonus : NetworkBehaviour, ITopOfCell
    {
        [field: SerializeField] private float _multiplier;
        public static float Multiplier;

        private TypeTopOfCell _currentType;

        public void Awake()
        {
            _currentType = TypeTopOfCell.Bonus;
            Multiplier = _multiplier;
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                TowerDefenseManager.Instance.AddBonus(this.gameObject);
            }
        }

        public TypeTopOfCell GetType()
        {
            return _currentType;
        }

        public GameObject ToGameObject()
        {
            return this.gameObject;
        }

        public void OnDestroy()
        {
            TilingGrid.RemoveElement(gameObject, transform.position);
            
            TowerDefenseManager.Instance.RemoveBonus(this.gameObject);
        }
    }
}