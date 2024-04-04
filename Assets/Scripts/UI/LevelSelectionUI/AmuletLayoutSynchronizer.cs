using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using Unity.Netcode;
using UnityEngine;

public class AmuletLayoutSynchronizer : NetworkBehaviour
{
    public static AmuletLayoutSynchronizer Instance { get; private set; }

    [field: SerializeField] public AmuletSelector AmuletSelector { get; private set; }

    private Dictionary<Loader.Scene, int[]> _amuletsIdsForScene;
    
    private void Awake()
    {
        Instance = this;

        _amuletsIdsForScene = new Dictionary<Loader.Scene, int[]>();
    }

    public int[] GetAmuletsIdsShownByServer(Loader.Scene levelScene)
    {
        if (_amuletsIdsForScene.TryGetValue(levelScene, out int[] amuletsIds))
        {
            return amuletsIds;
        }
        
        return new int[] {};
    }

    public void SaveServerSideAmuletsForLevel(Loader.Scene levelScene)
    {
        AmuletSaveLoad loader = new AmuletSaveLoad();

        int[] amuletsIdsForScene = loader.GetAmuletsIdsForScene(levelScene);
        
        SaveServerSideAmuletsForLevelServerRpc(levelScene, amuletsIdsForScene);
    }

    [ServerRpc()]
    private void SaveServerSideAmuletsForLevelServerRpc(Loader.Scene levelScene, int[] amuletsIdsForScene)
    {
        SaveServerSideAmuletsForLevelClientRpc(levelScene, amuletsIdsForScene);
    }

    [ClientRpc]
    private void SaveServerSideAmuletsForLevelClientRpc(Loader.Scene levelScene, int[] amuletsIdsForScene)
    {
        _amuletsIdsForScene.TryAdd(levelScene, amuletsIdsForScene);
    }
}