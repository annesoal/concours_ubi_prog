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
    [SerializeField] private Transform _monkeyBlockPlayerSpawn;
    
    private void SpawnMonkey(ulong clientId)
    {
        SpawnPlayerPrefab(clientId, playerMonkeyPrefab, _monkeyBlockPlayerSpawn);
    }
    
    [SerializeField] private Transform playerRobotPrefab;
    [SerializeField] private Transform _robotBlockPlayerSpawn;
    
    private void SpawnRobot(ulong clientId)
    {
        SpawnPlayerPrefab(clientId, playerRobotPrefab, _robotBlockPlayerSpawn);
    }

    private const string SPAWN_POINT_COMPONENT_ERROR =
        "Chaque spawn point de joueur doit avoir le component `BlockPlayerSpawn`";
    
    private void SpawnPlayerPrefab(ulong clientId, Transform playerPrefab, Transform spawnPoint)
    {
        Transform playerInstance = Instantiate(playerPrefab);

        NetworkObject playerNetworkObject = playerInstance.GetComponent<NetworkObject>();
        
        playerNetworkObject.SpawnAsPlayerObject(clientId, true);

        BlockPlayerSpawn blockPlayerSpawn;
        bool hasComponent = spawnPoint.TryGetComponent(out blockPlayerSpawn);
        
        if (hasComponent)
        {
            blockPlayerSpawn.SetPlayerOnBlock(playerPrefab);
        }
        else
        {
            Debug.LogError(SPAWN_POINT_COMPONENT_ERROR);
        }
    }
    
    private void DebugPlayerSpawnError(ulong clientId)
    {
        Debug.LogError("Player selection is `None` when in game, which is not a valid value !");
    }

}
