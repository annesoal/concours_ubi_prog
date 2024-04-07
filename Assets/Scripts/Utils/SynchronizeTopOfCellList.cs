using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using Unity.Netcode;
using UnityEngine;

public class SynchronizeTopOfCellList : NetworkBehaviour
{
    public static SynchronizeTopOfCellList Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Synchronize the first object of the topOfCell list form the cell to sync.
    /// </summary>
    public void SyncIndividualTopOfCell(Cell toSync)
    {
        ITopOfCell objectOnTop = toSync.ObjectsTopOfCell[0];

        GameObject objectOnTopGameObject = objectOnTop.ToGameObject();

        NetworkObject objectOnTopNetworkObject = objectOnTopGameObject.GetComponent<NetworkObject>();
        
        SyncIndividualTopOfCellClientRpc(objectOnTopNetworkObject);
    }

    [ClientRpc]
    private void SyncIndividualTopOfCellClientRpc(NetworkObjectReference objectOnTopNetworkRef)
    {
        if (IsServer) { return; }

        objectOnTopNetworkRef.TryGet(out NetworkObject objectOnTopNetworkObject);

        Cell toUpdate = TilingGrid.grid.GetCell(
            TilingGrid.LocalToGridPosition(objectOnTopNetworkObject.gameObject.transform.position)
        );
        
        toUpdate.ObjectsTopOfCell.Add(objectOnTopNetworkObject.GetComponent<ITopOfCell>());
        
        TilingGrid.grid.UpdateCell(toUpdate);
    }

    [ClientRpc]
    public void ClearAllClientTopOfCellsClientRpc()
    {
        if (IsServer) { return; }
        
        TilingGrid.grid.ClearAllTopOfCellsSync();
    }
}
