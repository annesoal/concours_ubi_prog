using System;
using System.Collections.Generic;
using System.Linq;
using Amulets;
using Enemies;
using Grid;
using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Type = Grid.Type;

/**
 * Classe responsable de la logique d'exécution du jeu.
 *
 * Elle fait le pont avec le GameMultiplayerManager (classe administractice d'interactions en ligne).
 * Elle s'occupe de la gestion des tours et du transfert de scènes entre la scène de jeu et les autres.
 */
public class TowerDefenseManager : NetworkBehaviour
{
    public enum State
    {
        WaitingToStart = 0,
        CountdownToStart,
        EnvironmentTurn,
        TacticalPause,
        EndOfGame
    }

    public static GameObject highlighter;
    private static List<Cell> DestinationCells;
    private List<GameObject> bonuses = new List<GameObject>();
    private bool gameWon = false;
    private IAManager _iaManager;

    [field: Header("Information du jeu")] // Nombre de round du que le niveau détient. Arrive a 0, les joueurs ont gagne.
    [field: SerializeField]
    public int totalRounds { get; private set; }

    // Énergie des joueurs disponible pour leurs actions lors de la pause tactique.
    [field: SerializeField] private int EnergyAvailable { get; set; }
    public int energyToUse;

    private float tacticalPauseDuration;


    [Header("Utility")] [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject _hightlighter;

    private int _playersHealth;

    [field: Header("Chrono de depart")]
    [field: SerializeField]
    public float CountDownToStartTimer { get; private set; }

    public int currentRoundNumber;

    [Header("Player Spawn")] [SerializeField]
    private Transform playerMonkeyPrefab;


    [field: SerializeField] public Transform MonkeyBlockPlayerSpawn { get; private set; }

    [SerializeField] private Transform playerRobotPrefab;
    [field: SerializeField] public Transform RobotBlockPlayerSpawn { get; private set; }

    [FormerlySerializedAs("selector")] [Header("Amulet")] [SerializeField]
    public AmuletSelector amuletSelector;

    [field: SerializeField] public Transform BossBlockSpawn { get; private set; }
    
    [Header("Next level data")]
    public NextLevelDataSO nextLevelDataSo;

    private readonly NetworkVariable<State> _currentState = new();

    private NetworkVariable<float> _currentTimer;
    private Dictionary<ulong, bool> _playerReadyToPassDictionary;

    // clientId and isReady pair
    private Dictionary<ulong, bool> _playerReadyToPlayDictionary;

    private Action<ulong>[] _spawnPlayerMethods;

    private Action[] _statesMethods;


    private List<Vector2Int> positions = new();
    public float TacticalPauseTimer => _currentTimer.Value;

    public static TowerDefenseManager Instance { get; private set; }

    private void Awake()
    {
        highlighter = _hightlighter;
        Instance = this;
        _currentTimer = new NetworkVariable<float>(tacticalPauseDuration);
        _iaManager = gameObject.AddComponent<IAManager>();
        _playerReadyToPlayDictionary = new Dictionary<ulong, bool>();
        _playerReadyToPassDictionary = new Dictionary<ulong, bool>();
        InitializeStatesMethods();
        InitializeSpawnPlayerMethods();
        currentRoundNumber = 0;
        // On assume que AmuletSelector.AmuletSelection a ete choisit avant !
        amuletSelector.SetAmulet();
        SetAmuletFieldsToGameFields();
    }

    private void SetAmuletFieldsToGameFields()
    {
        _playersHealth = amuletSelector.AmuletToUse.playersHealth;
        tacticalPauseDuration = amuletSelector.AmuletToUse.tacticalPauseTime;
        totalRounds = amuletSelector.AmuletToUse.numberOfTurns;
        EnergyAvailable = amuletSelector.AmuletToUse.energy;
        BaseTower.baseHealth = amuletSelector.AmuletToUse.towerBaseHealth;
        BaseTower.baseAttack = amuletSelector.AmuletToUse.towerBaseAttack;
        BaseTower.baseCost = amuletSelector.AmuletToUse.towerBaseCost;
        BaseTrap.baseCost = amuletSelector.AmuletToUse.trapBaseCost;
        Enemy.baseAttack = amuletSelector.AmuletToUse.enemyBaseAttack;
        Enemy.baseHealth = amuletSelector.AmuletToUse.enemyBaseHealth;
    }

    private void Start()
    {
        Instance.OnCurrentStateChanged += BeginTacticalPause;

        if (IsServer)
        {
            EnvironmentTurnManager.Instance.OnEnvironmentTurnEnded += EnvironmentManager_OnEnvironmentTurnEnded;
            Instance.OnCurrentStateChanged += EndGame;
        }

        //OnCurrentStateChanged += DebugStateChange;
        energyToUse = 0;
    }

    private void EndGame(object sender, OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == State.EndOfGame)
        {
            EndLevelClientRpc(gameWon);
        }
    }

    private void SaveProgress()
    {
        AmuletSaveLoad save = new AmuletSaveLoad();
        List<AmuletSO> unlockedAmulets = save.GetAmuletsForScene(Loader.TargetScene, amuletSelector.amulets);
        List<AmuletSO> unlockableAmulets = new List<AmuletSO>();

        AddUnlockableAmulets(unlockedAmulets, unlockableAmulets);

        if (unlockableAmulets.Count == 0)
        {
            foreach (var amulet in amuletSelector.amulets)
            {
                unlockableAmulets.Add(amulet);
            }
        }

        if (unlockableAmulets.Count != 0)
            unlockedAmulets.Add(unlockableAmulets[0]);

        save.SaveSceneWithAmulets(Loader.TargetScene, unlockedAmulets.ToArray());
    }

    private void AddUnlockableAmulets(List<AmuletSO> unlockedAmulets, List<AmuletSO> unlockableAmulets)
    {
        foreach (var amulet in amuletSelector.amulets)
        {
            foreach (var unlockedAmulet in unlockedAmulets)
            {
                {
                    if (unlockedAmulet.ID != amulet.ID)
                        unlockableAmulets.Add(amulet);
                }
            }
        }
    }

    [ClientRpc]
    private void EndLevelClientRpc(bool won)
    {
        ShowEndGameScreen();
    }

    private void Update()
    {
        if (IsServer) _statesMethods[(int)_currentState.Value]();
    }

    private void InitializeStatesMethods()
    {
        _statesMethods = new Action[]
        {
            WaitForPlayerReadyToPlay,
            ProgressCountDownToStartTimer,
            PlayEnvironmentTurn,
            ProgressTacticalTimer,
            () => { }
        };
    }

    private void DebugStateChange(object sender, OnCurrentStateChangedEventArgs e)
    {
        Debug.Log(e.newValue);

        InputManager.Instance.DisablePlayerInputMap();
    }

    private void BeginTacticalPause(object sender, OnCurrentStateChangedEventArgs e)
    {
        InputManager.Instance.EnablePlayerInputMap();

        if (e.newValue == State.TacticalPause)
        {
            IncreaseRoundNumber();

            energyToUse = EnergyAvailable;
            Player.LocalInstance.ResetPlayer(EnergyAvailable);
            _playerReadyToPassDictionary = new Dictionary<ulong, bool>();

            if (IsServer)
            {
                _currentTimer.Value = tacticalPauseDuration;
                CentralizedInventory.Instance.CashBonus();
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        _currentState.OnValueChanged += TowerDefenseManager_CurrentState_OnValueChanged;

        if (IsServer) NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += NetworkManager_OnLoadEventCompleted;
    }

    private void WaitForPlayerReadyToPlay()
    {
        if (PlayersAreReadyToPlay()) GoToSpecifiedState(State.CountdownToStart);
    }

    private void ProgressCountDownToStartTimer()
    {
        ProgressCountDownToStartTimerClientRpc(Time.deltaTime);

        CountDownToStartTimer -= Time.deltaTime;

        if (CountDownToStartTimer <= 0f) GoToSpecifiedState(State.EnvironmentTurn);
    }

    [ClientRpc]
    private void ProgressCountDownToStartTimerClientRpc(float deltaTime)
    {
        if (IsServer) return;

        CountDownToStartTimer -= Time.deltaTime;
    }

    private void PlayEnvironmentTurn()
    {
    }

    private void EnvironmentManager_OnEnvironmentTurnEnded(object sender, EventArgs e)
    {
        CheckEnemiesAtDestinationCells();
        CleanBonuses();
        TilingGrid.grid.SyncAllTopOfCells();
        
        if (_playersHealth < 1)
        {
            gameWon = false;
            GoToSpecifiedState(State.EndOfGame);
        }

        if (currentRoundNumber >= totalRounds)
        {
            gameWon = true;
            GoToSpecifiedState(State.EndOfGame);
        }
        else
            GoToSpecifiedState(State.TacticalPause);
    }

    private void ProgressTacticalTimer()
    {
        if (AllRoundsAreDone())
        {
            gameWon = true;
            GoToSpecifiedState(State.EndOfGame);
        }

        _currentTimer.Value -= Time.deltaTime;

        if (_currentTimer.Value <= 0f || PlayersAreReadyToPass()) GoToSpecifiedState(State.EnvironmentTurn);
    }

    public event EventHandler OnVictory;
    public event EventHandler OnDefeat;
    private void ShowEndGameScreen()
    {
        InputManager.Instance.DisablePlayerInputAction();
        
        if (gameWon)
        {
            OnVictory?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnDefeat?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool AllRoundsAreDone()
    {
        return currentRoundNumber == totalRounds;
    }

    public event EventHandler OnRoundNumberIncreased;

    private void IncreaseRoundNumber()
    {
        currentRoundNumber += 1;
        OnRoundNumberIncreased?.Invoke(this, EventArgs.Empty);
    }

    private bool PlayersAreReadyToPlay()
    {
        var areReady = true;

        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            if (ConnectedPlayerIsNotReady(clientId))
            {
                areReady = false;
                break;
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
        if (!_playerReadyToPlayDictionary.ContainsKey(clientIdOfPlayerReady))
            _playerReadyToPlayDictionary.Add(clientIdOfPlayerReady, true);
    }

    private bool ConnectedPlayerIsNotReady(ulong clientIdOfPlayer)
    {
        return !_playerReadyToPlayDictionary.ContainsKey(clientIdOfPlayer) ||
               !_playerReadyToPlayDictionary[clientIdOfPlayer];
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

    private void TowerDefenseManager_CurrentState_OnValueChanged(State previousValue, State newValue)
    {
        OnCurrentStateChanged?.Invoke(this, new OnCurrentStateChangedEventArgs
        {
            previousValue = previousValue,
            newValue = newValue
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
        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            var characterSelection =
                GameMultiplayerManager.Instance.GetCharacterSelectionFromClientId(clientId);

            _spawnPlayerMethods[(int)characterSelection](clientId);
        }
    }


    private void SpawnMonkey(ulong clientId)
    {
        Debug.Log((clientId == NetworkManager.ServerClientId) + " IN MONKEY SPAWN");
        SpawnPlayerPrefab(clientId, playerMonkeyPrefab);
    }


    private void SpawnRobot(ulong clientId)
    {
        Debug.Log((clientId == NetworkManager.ServerClientId) + " IN ROBOT SPAWN");
        SpawnPlayerPrefab(clientId, playerRobotPrefab);
    }

    private void SpawnPlayerPrefab(ulong clientId, Transform playerPrefab)
    {
        var playerInstance = Instantiate(playerPrefab);

        var playerNetworkObject = playerInstance.GetComponent<NetworkObject>();

        playerNetworkObject.SpawnAsPlayerObject(clientId, true);
    }

    private void DebugPlayerSpawnError(ulong clientId)
    {
        Debug.LogError("Player selection is `None` when in game, which is not a valid value !");
    }


    private bool PlayersAreReadyToPass()
    {
        var areReady = true;

        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            if (ConnectedPlayerIsNotReadyToPass(clientId))
            {
                areReady = false;
                break;
            }

        return areReady;
    }

    public void SetPlayerReadyToPass(bool value)
    {
        SetPlayerReadyToPassServerRpc(value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyToPassServerRpc(bool value, ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyToPassClientRpc(value, serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void SetPlayerReadyToPassClientRpc(bool value, ulong clientIdOfPlayerReady)
    {
        try
        {
            _playerReadyToPassDictionary.Add(clientIdOfPlayerReady, value);
        }
        catch (ArgumentException)
        {
            _playerReadyToPassDictionary[clientIdOfPlayerReady] = value;
        }
    }

    private bool ConnectedPlayerIsNotReadyToPass(ulong clientIdOfPlayer)
    {
        return !_playerReadyToPassDictionary.ContainsKey(clientIdOfPlayer) ||
               !_playerReadyToPassDictionary[clientIdOfPlayer];
    }

    private static void CheckEnemiesAtDestinationCells()
    {
        DestinationCells = TilingGrid.grid.GetCellsOfType(Type.EnemyDestination);
        foreach (var cell in DestinationCells)
        {
            if (cell.ContainsEnemy())
            {
                var enemies = cell.GetEnemies();
                foreach (var enemy in enemies)
                {
                    enemy.RemoveInGame();
                    TilingGrid.RemoveElement(enemy.ToGameObject(), cell.position);
                    Destroy(enemy.ToGameObject());
                }

                Instance._playersHealth--;
            }
        }
    }

    public static void ResetStaticData()
    {
        DestinationCells = null;
    }

    public class OnCurrentStateChangedEventArgs : EventArgs
    {
        public State newValue;
        public State previousValue;
    }

    public void AddBonus(GameObject bonus)
    {
        this.bonuses.Add(bonus);
    }

    public void RemoveBonus(GameObject bonus)
    {
        this.bonuses.Remove(bonus);
    }

    private void CleanBonuses()
    {
        foreach (var bonus in bonuses)
        {
            Destroy(bonus);
        }
    }

    public override void OnDestroy()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= NetworkManager_OnLoadEventCompleted;
        base.OnDestroy();
    }
}