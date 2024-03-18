using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Unity.Netcode;
using UnityEngine;

public class SynchronizeBuilding : NetworkBehaviour
{
    public static SynchronizeBuilding Instance { get; private set; }

    [SerializeField] private BuildableObjectsListSO allBuildableObjectSO;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnBuildableObject(BuildableObjectSO toBuild, Cell buildableBlock)
    {
        int indexOfBuildableObjectSO = allBuildableObjectSO.list.IndexOf(toBuild);

        if (indexOfBuildableObjectSO == -1)
        {
            Debug.LogError("No matching index found for BuildableObjectSO !\n" +
                           "Maybe the buildableObjectList is missing a buildableObject.");
        }
        
        Vector3 positionToBuild = TilingGrid.GridPositionToLocal(buildableBlock.position);
        
        SpawnBuildableObjectServerRpc(indexOfBuildableObjectSO, positionToBuild);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBuildableObjectServerRpc(int indexOfBuildableObjectSO, Vector3 positionToBuild)
    {
        GameObject instance = Instantiate(allBuildableObjectSO.list[indexOfBuildableObjectSO].prefab);
        
        instance.GetComponent<IBuildable>().Build(positionToBuild);
        
        Debug.Log("Before Spawn");
        NetworkObject buildableObjectNetworkObject = instance.GetComponent<NetworkObject>();
        buildableObjectNetworkObject.Spawn(true);
        Debug.Log("After Spawn");
        
        SpawnBuildableObjectClientRpc(buildableObjectNetworkObject, positionToBuild);
    }

    [ClientRpc]
    private void SpawnBuildableObjectClientRpc(NetworkObjectReference buildableObjectNetworkObject, Vector3 positionToBuild)
    {
        buildableObjectNetworkObject.TryGet(out NetworkObject buildableObjectNetwork);
        buildableObjectNetwork.GetComponent<IBuildable>().Build(positionToBuild);
        
        Vector2Int cellPosition = TilingGrid.LocalToGridPosition(positionToBuild);
        Cell cellWithNewObject = TilingGrid.grid.GetCell(cellPosition);
        // TODO set cellWithNewObject.hasObjectOnTop = true
    }
    
    public BuildableObjectsListSO GetAllBuildableObjectSo()
    {
        return allBuildableObjectSO;
    }
}
