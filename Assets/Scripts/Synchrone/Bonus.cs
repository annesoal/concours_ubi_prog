using System;
using Grid.Interface;
using Unity.Netcode;
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
        
        public TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Bonus;
        }

        public GameObject ToGameObject()
        {
            return this.gameObject;
        }
    }
}