using System;
using Grid;
using Unity.Netcode;
using UnityEngine;

public class SynchronizeBuilding : NetworkBehaviour
{
    public static SynchronizeBuilding Instance { get; private set; }

    [SerializeField] public BuildableObjectsListSO allBuildableObjectSO;

    private void Awake()
    {
        Instance = this;
    }
    
    public void OverrideBuildingCosts()
    {
        BuildableObjectsListSO  listSO = Instance.allBuildableObjectSO;
        var pair = listSO.list[0].materialAndQuantityPairs[0];
        pair.quantityOfMaterialRequired = BasicTower.BasicTowerCost;
        listSO.list[0].materialAndQuantityPairs[0] = pair;
        
        pair = listSO.list[1].materialAndQuantityPairs[0];
        pair.quantityOfMaterialRequired = BasicTrap.SetCost;
        listSO.list[1].materialAndQuantityPairs[0] = pair;
        
        pair = listSO.list[2].materialAndQuantityPairs[0];
        pair.quantityOfMaterialRequired = ZombotTrap.SetCost;
        listSO.list[2].materialAndQuantityPairs[0] = pair;
        
    }

    public void SpawnBuildableObject(BuildableObjectSO toBuild, Cell buildableBlock)
    {
        int indexOfBuildableObjectSO = allBuildableObjectSO.list.IndexOf(toBuild);

        if (indexOfBuildableObjectSO == -1)
        {
            Debug.LogError("No matching index found for BuildableObjectSO !\n" +
                           "Maybe the buildableObjectList is missing a buildableObject.");
        }
        
        SpawnBuildableObjectServerRpc(indexOfBuildableObjectSO, buildableBlock.position);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBuildableObjectServerRpc(int indexOfBuildableObjectSO, Vector2Int positionToBuild)
    {
        TakeResourcesFromInventory(allBuildableObjectSO.list[indexOfBuildableObjectSO]);
        
        GameObject instance = Instantiate(allBuildableObjectSO.list[indexOfBuildableObjectSO].prefab);
        
        NetworkObject buildableObjectNetworkObject = instance.GetComponent<NetworkObject>();
        
        buildableObjectNetworkObject.GetComponent<IBuildable>().Build(positionToBuild);
        
        buildableObjectNetworkObject.Spawn(true);
        
        SpawnBuildableObjectClientRpc(buildableObjectNetworkObject, positionToBuild);
    }

    private void TakeResourcesFromInventory(BuildableObjectSO buildableObjectSo)
    {
        CentralizedInventory.Instance.DecreaseResourceForBuilding(buildableObjectSo);
    }

    public event EventHandler<OnBuildingBuiltEventArgs> OnBuildingBuilt;
    public class OnBuildingBuiltEventArgs : EventArgs
    {
        public Vector2Int BuildingPosition;
    }
    
    [ClientRpc]
    private void SpawnBuildableObjectClientRpc(NetworkObjectReference buildableObjectNetworkObject, Vector2Int positionToBuild)
    {
        buildableObjectNetworkObject.TryGet(out NetworkObject buildableObjectNetwork);
        if (!IsServer)
        {
            buildableObjectNetwork.GetComponent<IBuildable>().SynchBuild();
        }
        
        OnBuildingBuilt?.Invoke(this, new OnBuildingBuiltEventArgs
        {
            BuildingPosition = positionToBuild,
        });
    }
    
    public BuildableObjectsListSO GetAllBuildableObjectSo()
    {
        return allBuildableObjectSO;
    }
}
