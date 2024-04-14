using Grid;
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
            return TypeTopOfCell.Malus;
        }


        public GameObject ToGameObject()
        {
            return gameObject;
        }
        
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                TowerDefenseManager.Instance.AddMalus(this.gameObject);
            }
        }

        public void OnDestroy()
        {
            Vector2Int gridPosition = TilingGrid.LocalToGridPosition(transform.position);
            Cell toUpdate = TilingGrid.grid.GetCell(gridPosition);
            toUpdate.ObjectsTopOfCell.Remove(this);
            TilingGrid.grid.UpdateCell(toUpdate);
            TowerDefenseManager.Instance.RemoveMalus(this.gameObject);
            
        }
    }
}