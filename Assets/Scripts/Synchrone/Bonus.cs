using System;
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

        public void Awake()
        {
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
            return TypeTopOfCell.Bonus;
        }

        public GameObject ToGameObject()
        {
            return this.gameObject;
        }

        public void OnDestroy()
        {
            TowerDefenseManager.Instance.RemoveBonus(this.gameObject);
        }
    }
}