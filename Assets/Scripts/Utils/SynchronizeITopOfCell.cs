using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using Unity.Netcode;
using UnityEngine;

public class SynchronizeITopOfCell : NetworkBehaviour
{
    public static SynchronizeITopOfCell Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public event EventHandler<OnElementSynchronizedEventArgs> OnElementSynchronized;
    public class OnElementSynchronizedEventArgs : EventArgs
    {
        public Cell ToUpdate;
    }
    
    public void SynchronizeAddingElement(GameObject toAdd, Cell toSync)
    {
        SynchronizeAddingElementServerRpc(toAdd, toSync.position);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SynchronizeAddingElementServerRpc(NetworkObjectReference toAddRef, Vector2Int cellPositionToSync)
    {
        SynchronizeAddingElementClientRpc(toAddRef, cellPositionToSync);
    }
    
    [ClientRpc]
    private void SynchronizeAddingElementClientRpc(NetworkObjectReference toAddRef, Vector2Int cellPositionToSync)
    {
        toAddRef.TryGet(out NetworkObject toSyncNetworkObject);

        GameObject toAdd = toSyncNetworkObject.gameObject;

        Cell toSync = TilingGrid.grid.GetCell(cellPositionToSync);
        
        toSync.AddGameObject(toAdd.GetComponent<ITopOfCell>());
        
        
        OnElementSynchronized?.Invoke(this, new OnElementSynchronizedEventArgs
        {
            ToUpdate = toSync,
        });
    }
    
    public void SynchronizeRemovingElement(GameObject toRemove, Cell toSync)
    {
        toSync.ObjectsTopOfCell.Remove(toRemove.GetComponent<ITopOfCell>());
        
        // TODO lancer event côté client rpc pour update cell dans grid.
        OnElementSynchronized?.Invoke(this, new OnElementSynchronizedEventArgs
        {
            ToUpdate = toSync,
        });
    }
}
