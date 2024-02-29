using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Blocks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Classe responsable de la logique d'exécution du jeu.
 * 
 * Elle fait le pont avec le GameMultiplayerManager (classe administractice d'interactions en ligne).
 * Elle s'occupe de la gestion des tours et du transfert de scènes entre la scène de jeu et les autres.
 */
public class TowerDefenseManager : NetworkBehaviour
{
    private enum State
    {
        WaitingToStart = 0,
        CountdownToStart,
        EnvironmentTurn,
        TacticalPause,
        EndOfGame,
    }

    public static TowerDefenseManager Instance { get; private set; }

    private NetworkVariable<State> _currentState = new NetworkVariable<State>(State.WaitingToStart);
    
    private void Awake()
    {
        Instance = this;
        
        InitializeSpawnPlayerMethods();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += NetworkManager_OnLoadEventCompleted;
        }
    }

    private void NetworkManager_OnLoadEventCompleted
        (string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedout)
    {
        CarryOutSpawnPlayersProcedure();
    }

    private void CarryOutSpawnPlayersProcedure()
    {
        try
        {
            SpawnPlayers();
        }
        catch (NoMatchingClientIdFoundException e)
        {
            Debug.LogError(e);
        }
    }

    private Action<ulong>[] _spawnPlayerMethods;

    private void InitializeSpawnPlayerMethods()
    {
        _spawnPlayerMethods = new Action<ulong>[]
        {
            SpawnMonkey,
            SpawnRobot,
            DebugPlayerSpawnError
        };
    }
    
    /**
     * Throws NoMatchingClientIdFoundException.
     */
    private void SpawnPlayers()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            CharacterSelectUI.CharacterId characterSelection =
                GameMultiplayerManager.Instance.GetCharacterSelectionFromClientId(clientId);

            _spawnPlayerMethods[(int)characterSelection](clientId);
        }
    }

    [Header("Player Spawn")]
    [SerializeField] private Transform playerMonkeyPrefab;
    [field: SerializeField] public Transform MonkeyBlockPlayerSpawn { get; private set; }

    private void SpawnMonkey(ulong clientId)
    {
        Debug.Log((clientId == NetworkManager.ServerClientId) + " IN MONKEY SPAWN");
        SpawnPlayerPrefab(clientId, playerMonkeyPrefab);
    }
    
    [SerializeField] private Transform playerRobotPrefab;
    [field: SerializeField] public Transform RobotBlockPlayerSpawn { get; private set; }

    private void SpawnRobot(ulong clientId)
    {
        Debug.Log((clientId == NetworkManager.ServerClientId) + " IN ROBOT SPAWN");
        SpawnPlayerPrefab(clientId, playerRobotPrefab);
    }
    
    private void SpawnPlayerPrefab(ulong clientId, Transform playerPrefab)
    {
        Transform playerInstance = Instantiate(playerPrefab);

        NetworkObject playerNetworkObject = playerInstance.GetComponent<NetworkObject>();
        
        playerNetworkObject.SpawnAsPlayerObject(clientId, true);
    }
    
    private void DebugPlayerSpawnError(ulong clientId)
    {
        Debug.LogError("Player selection is `None` when in game, which is not a valid value !");
    }

}
