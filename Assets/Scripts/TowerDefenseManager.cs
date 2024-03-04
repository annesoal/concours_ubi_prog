using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Blocks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/**
 * Classe responsable de la logique d'exécution du jeu.
 * 
 * Elle fait le pont avec le GameMultiplayerManager (classe administractice d'interactions en ligne).
 * Elle s'occupe de la gestion des tours et du transfert de scènes entre la scène de jeu et les autres.
 */
public class TowerDefenseManager : NetworkBehaviour
{
    [Header("Information du jeu")]
    // Nombre de round du que le niveau detient. Arrive a 0, les joueurs ont gagne.
    [SerializeField] private int totalRounds;
    
    [Header("Pause Tactique")]
    [SerializeField] private float tacticalPauseDuration;

    public enum State
    {
        WaitingToStart = 0,
        CountdownToStart,
        EnvironmentTurn,
        TacticalPause,
        EndOfGame,
    }

    private Action[] _statesMethods;
    private void InitializeStatesMethods()
    {
        _statesMethods = new Action[]
        {
            WaitForPlayerReadyToPlay,
            ProgressCountDownToStartTimer,
            PlayEnvironmentTurn,
            ProgressTacticalTimer,
            () => { },
        };
    }

    public static TowerDefenseManager Instance { get; private set; }

    private NetworkVariable<State> _currentState = new NetworkVariable<State>(State.WaitingToStart);
    
    // clientId and isReady pair
    private Dictionary<ulong, bool> _playerReadyToPlayDictionary;

    private void Awake()
    {
        Instance = this;

        _playerReadyToPlayDictionary = new Dictionary<ulong, bool>();
        
        InitializeStatesMethods();
        InitializeSpawnPlayerMethods();

        _currentRoundNumber = totalRounds;
    }

    public override void OnNetworkSpawn()
    {
        _currentState.OnValueChanged += TowerDefenseManager_CurrentState_OnValueChanged;
        
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += NetworkManager_OnLoadEventCompleted;
        }
    }

    private void Update()
    {
        if (IsServer)
        {
            _statesMethods[(int)_currentState.Value]();
        }
    }
    
    private void WaitForPlayerReadyToPlay()
    {
        if (PlayersAreReadyToPlay())
        {
            GoToSpecifiedState(State.CountdownToStart);
        }
    }

    [field: Header("Chrono de depart")]
    [field: SerializeField] public float CountDownToStartTimer { get; private set; }
    
    private void ProgressCountDownToStartTimer()
    {
        ProgressCountDownToStartTimerClientRpc(Time.deltaTime);
        
        CountDownToStartTimer -= Time.deltaTime;

        if (CountDownToStartTimer <= 0f)
        {
            GoToSpecifiedState(State.EnvironmentTurn);
        }
    }
    
    [ClientRpc()]
    private void ProgressCountDownToStartTimerClientRpc(float deltaTime)
    {
        if (IsServer) { return; }

        CountDownToStartTimer -= Time.deltaTime;
    }
    
    private bool _isEnvironmentTurnNotCalled = true;
    private void PlayEnvironmentTurn()
    {
        if (! _isEnvironmentTurnNotCalled)
        {
            _isEnvironmentTurnNotCalled = false;
            // TODO Centraliser ça dans EnvironmentManager
            // Connexion à un event qui se lance lorsque le EnvironmentManager a fini (retour en pause tactique ou en gameOver)
            /**
             * Effecuter le Spawn d'enemis (prévu avant, consultable par les joueurs)
             * 
             * AIM.ComputePaths()
             * while (hasEnergy())
             * {
             *     AIM.PlaySubordinatesTurn();
             *
             *     TowerManager.PlayTowersTurn();
             * 
             *     currentEnvironmentEnergy--;
             * }
             * 
             * currentEnvironmentEnergy = INITIAL_CURRENT_ENERGY;
             * GoToSpecifiedState(State.TacticalPause);
             */
        }
    }
    
    private void ProgressTacticalTimer()
    {
        if (AllRoundsAreDone())
        {
            GoToSpecifiedState(State.EndOfGame);
        }
        
        tacticalPauseDuration -= Time.deltaTime;
        
        if (tacticalPauseDuration <= 0f)
        {
            IncreaseRoundNumber();
            GoToSpecifiedState(State.EnvironmentTurn);
        }
    }

    private bool AllRoundsAreDone()
    {
        return _currentRoundNumber == 0;
    }

    private int _currentRoundNumber;
    
    private void IncreaseRoundNumber()
    {
        _currentRoundNumber += 1;
    }

    private bool PlayersAreReadyToPlay()
    {
        bool areReady = true;
        
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (ConnectedPlayerIsNotReady(clientId))
            {
                areReady = false;
                break;
            }
        }

        return areReady;
    }
    
    public void SetPlayerReadyToPlay()
    {
        SetPlayerReadyToPlayServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyToPlayServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyToPlayClientRpc(serverRpcParams.Receive.SenderClientId);
    }
    
    [ClientRpc]
    private void SetPlayerReadyToPlayClientRpc(ulong clientIdOfPlayerReady)
    {
        _playerReadyToPlayDictionary.Add(clientIdOfPlayerReady, true);
    }

    private bool ConnectedPlayerIsNotReady(ulong clientIdOfPlayer)
    {
        return ! _playerReadyToPlayDictionary.ContainsKey(clientIdOfPlayer) || 
               ! _playerReadyToPlayDictionary[clientIdOfPlayer];
    }
    
    private void GoToSpecifiedState(State specified)
    {
        _currentState.Value = specified;
    }
    
    private void GoToNextState()
    {
        _currentState.Value += 1 % _statesMethods.Length;
    }

    private void NetworkManager_OnLoadEventCompleted
        (string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedout)
    {
        CarryOutSpawnPlayersProcedure();
    }

    public event EventHandler<OnCurrentStateChangedEventArgs> OnCurrentStateChanged;
    public class OnCurrentStateChangedEventArgs : EventArgs
    {
        public State previousValue;
        public State newValue;
    }
    private void TowerDefenseManager_CurrentState_OnValueChanged(State previousValue, State newValue)
    {
        OnCurrentStateChanged?.Invoke(this, new OnCurrentStateChangedEventArgs
        {
            previousValue = previousValue,
            newValue = newValue,
        });
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
