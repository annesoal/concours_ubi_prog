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
        
        SpawnBuildableObjectServerRpc(indexOfBuildableObjectSO, buildableBlock.position);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBuildableObjectServerRpc(int indexOfBuildableObjectSO, Vector2Int positionToBuild)
    {
        GameObject instance = Instantiate(allBuildableObjectSO.list[indexOfBuildableObjectSO].prefab);
        
        NetworkObject buildableObjectNetworkObject = instance.GetComponent<NetworkObject>();
        buildableObjectNetworkObject.Spawn(true);
        
        SpawnBuildableObjectClientRpc(buildableObjectNetworkObject, positionToBuild);
    }

    [ClientRpc]
    private void SpawnBuildableObjectClientRpc(NetworkObjectReference buildableObjectNetworkObject, Vector2Int positionToBuild)
    {
        buildableObjectNetworkObject.TryGet(out NetworkObject buildableObjectNetwork);
        buildableObjectNetwork.GetComponent<IBuildable>().Build(positionToBuild);
    }
    
    public BuildableObjectsListSO GetAllBuildableObjectSo()
    {
        return allBuildableObjectSO;
    }
}
