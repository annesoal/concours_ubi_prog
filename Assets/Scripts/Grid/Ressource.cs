using Grid.Interface;
using Unity.Netcode;
using UnityEngine;

namespace Grid
{
    public class Ressource : NetworkBehaviour, ITopOfCell
    {

        public static float SpawnRate;
        [SerializeField] private float topOfCell = 0.72f;

        [field: SerializeField] public BuildingMaterialSO BuildingMaterialSO { get; private set; }

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
