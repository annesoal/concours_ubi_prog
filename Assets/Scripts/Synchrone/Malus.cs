using Grid.Interface;
using Unity.Netcode;
using UnityEngine;

namespace Synchrone
{
    public class Malus : NetworkBehaviour, ITopOfCell
    {
        
        [field: SerializeField] public int value { get; private set; }
        [field: SerializeField] public BuildingMaterialSO BuildingMaterialSO { get; private set; }
        public TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Bonus;
        }

        public GameObject ToGameObject()
        {
            return gameObject;
        }
    }
}