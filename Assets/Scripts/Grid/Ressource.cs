using System;
using Grid.Interface;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grid
{
    public class Ressource : NetworkBehaviour, ITopOfCell
    {
        [SerializeField] private float topOfCell = 0.72f;

        public new TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Resource;
        }

        public GameObject ToGameObject()
        {
            return this.gameObject;
        }
    }
}