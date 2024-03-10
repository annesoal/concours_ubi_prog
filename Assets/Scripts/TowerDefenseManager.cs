using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Grid.Blocks;
using Managers;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

/**
 * Classe responsable de la logique d'exécution du jeu.
 * 
 * Elle fait le pont avec le GameMultiplayerManager (classe administractice d'interactions en ligne).
 * Elle s'occupe de la gestion des tours et du transfert de scènes entre la scène de jeu et les autres.
 */
public class TowerDefenseManager : NetworkBehaviour
{
    
    private List<Vector2Int> positions = new List<Vector2Int>();
    [Header("Information du jeu")]
    // Nombre de round du que le niveau détient. Arrive a 0, les joueurs ont gagne.
    [SerializeField] private int totalRounds;

    // Énergie des joueurs disponible pour leurs actions lors de la pause tactique.
    [field: SerializeField] public int EnergyAvailable { get; private set; }

    [Header("Pause Tactique")]
    [SerializeField] private float tacticalPauseDuration;

    private float _tacticalPauseTimer;

    [Header("Obstacles")] 
    [SerializeField] private GameObject obstacle;
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

        currentRoundNumber = 0;
    }

    private void Start()
    {
        if (EnvironmentTurnManager.Instance != null)
            EnvironmentTurnManager.Instance.OnEnvironmentTurnEnded += EnvironmentManager_OnEnvironmentTurnEnded;

        Instance.OnCurrentStateChanged += SetTimer;
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
        if (_isEnvironmentTurnNotCalled)
        {
            _isEnvironmentTurnNotCalled = false;
            EnvironmentTurnManager.Instance.EnableEnvironmentTurn(EnergyAvailable);
        }
    }

    private void EnvironmentManager_OnEnvironmentTurnEnded(object sender, EventArgs e)
    {
        Debug.Log("curr RN : " + currentRoundNumber + " tot : " + totalRounds );
        if (currentRoundNumber >= totalRounds)
        {
            GoToSpecifiedState(State.EndOfGame);
        }
        else
        {
            GoToSpecifiedState(State.TacticalPause);
        }
        
        _isEnvironmentTurnNotCalled = true;
    }

    private void SetTimer(object sender, OnCurrentStateChangedEventArgs onCurrentStateChangedEventArgs)
    {
        if (onCurrentStateChangedEventArgs.newValue == State.TacticalPause)
        {
            _tacticalPauseTimer = tacticalPauseDuration;
        }
    }
    private void ProgressTacticalTimer()
    {
        if (AllRoundsAreDone())
        {
            GoToSpecifiedState(State.EndOfGame);
        }
        
        _tacticalPauseTimer -= Time.deltaTime;
        
        if (_tacticalPauseTimer <= 0f)
        {
            IncreaseRoundNumber();
            GoToSpecifiedState(State.EnvironmentTurn);
        }
    }

    private bool AllRoundsAreDone()
    {
        return currentRoundNumber == totalRounds;
    }

     public int currentRoundNumber;
    
    private void IncreaseRoundNumber()
    {
        currentRoundNumber += 1;
    }

    // Begin Ready 
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
    // End Ready to play
    
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

    // Spawn players methods
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
