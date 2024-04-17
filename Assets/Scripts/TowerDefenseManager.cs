using System;
using System.Collections.Generic;
using System.Linq;
using Amulets;
using Building.Towers;
using Building.Traps;
using Enemies;
using Enemies.Attack;
using Enemies.Basic;
using Enemies.Boss;
using Grid;
using Grid.Blocks;
using Managers;
using Sound;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private List<GameObject> maluses = new List<GameObject>();
    private bool gameWon = false;
    private IAManager _iaManager;

    private bool _isFirstTurn = true;

    public bool IsFirstTurn
    {
        get
        {
            if (_isFirstTurn)
            {
                var savedValue = _isFirstTurn;
                _isFirstTurn = false;
                return savedValue;
            }

            return _isFirstTurn;
        }
    }

    [field: Header("Information du jeu")] // Nombre de round du que le niveau détient. Arrive a 0, les joueurs ont gagne.
    public static int TotalRounds;

    // Énergie des joueurs disponible pour leurs actions lors de la pause tactique.
    public int energyToUse;

    private static float TacticalPauseDuration;

    public float CountDownToStartTimer { get; private set; }
    public static int StartingBackEndTurn;

    public int currentRoundNumber;

    [Header("Player Spawn")] [SerializeField]
    private Transform playerMonkeyPrefab;

    [field: SerializeField] public Transform MonkeyBlockPlayerSpawn { get; private set; }

    [SerializeField] private Transform playerRobotPrefab;
    [field: SerializeField] public Transform RobotBlockPlayerSpawn { get; private set; }


    [field: SerializeField] public Transform BossPrefab { get; private set; }
    
    [Header("Next level data")]
    public NextLevelDataSO nextLevelDataSo;
    
    
    [Header("AdditionAmulet")]
    [SerializeField] private AdditionAmuletSO defaultAmulet;
    
    /// Reset manually.
    public static AdditionAmuletSO PlayerAmuletSelection;

    [SerializeField] private List<SpawnerBlock> listOfSpawners;
    private readonly NetworkVariable<State> _currentState = new();
    public State CurrentState => _currentState.Value;

    [SerializeField] public AmuletSO amuletSO;
    private NetworkVariable<float> _currentTimer;
    private Dictionary<ulong, bool> _playerReadyToPassDictionary;

    // clientId and isReady pair
    private Dictionary<ulong, bool> _playerReadyToPlayDictionary;

    private Action<ulong>[] _spawnPlayerMethods;

    private Action[] _statesMethods;
    
    private List<Vector2Int> positions = new();
    public float TacticalPauseTimer => _currentTimer.Value;

    public static TowerDefenseManager Instance { get; private set; }
    private TowerManager _towerManager;
    private TrapManager _trapManager;

    private void Awake()
    {
        Instance = this;
        _currentTimer = new NetworkVariable<float>(TacticalPauseDuration);
        _iaManager = gameObject.AddComponent<IAManager>();
        _trapManager = gameObject.AddComponent<TrapManager>();
        _towerManager = gameObject.AddComponent<TowerManager>();
        _playerReadyToPlayDictionary = new Dictionary<ulong, bool>();
        _playerReadyToPassDictionary = new Dictionary<ulong, bool>();
        InitializeStatesMethods();
        InitializeSpawnPlayerMethods();
        currentRoundNumber = 0;
        SetAmuletFieldsToGameFields();
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
        this.gameObject.AddComponent<EnemySpawnerManager>().SetSpawners(listOfSpawners);
    }

    private void EndGame(object sender, OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == State.EndOfGame)
        {
            EndLevelClientRpc(gameWon);
        }
    }

    [ClientRpc]
    private void EndLevelClientRpc(bool victory)
    {
        ShowEndGameScreen(victory);
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

            energyToUse = Player.Energy;
            Player.LocalInstance.ResetPlayer(Player.Energy);
            _playerReadyToPassDictionary = new Dictionary<ulong, bool>();

            if (IsServer)
            {
                _currentTimer.Value = TacticalPauseDuration;
                CentralizedInventory.Instance.CashBonus();
                BigBossEnemy.Instance.SpawnMalusOnGrid();
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
        CentralizedInventory.Instance.Initialize();
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
        CleanBonuses();
        CleanMaluses();
        TilingGrid.grid.SyncAllTopOfCells();
        
        if (Player.Health < 1)
        {
            gameWon = false;
            GoToSpecifiedState(State.EndOfGame);
        }

        if (currentRoundNumber >= TotalRounds && HasNoEnemyInGame())
        {
            gameWon = true;
            GoToSpecifiedState(State.EndOfGame);
        }
        else
            GoToSpecifiedState(State.TacticalPause);
    }

    private bool HasNoEnemyInGame()
    {
        List<GameObject> enemies = Enemy.GetEnemiesInGame();
        if (enemies == null)
        {
            return true;
        }

        return enemies.Count == 0;
    }
    
    private void ProgressTacticalTimer()
    {
        if (AllRoundsAreDone() && HasNoEnemyInGame())
        {
            gameWon = true;
            GoToSpecifiedState(State.EndOfGame);
        }

        _currentTimer.Value -= Time.deltaTime;

        if (_currentTimer.Value <= 0f || PlayersAreReadyToPass()) GoToSpecifiedState(State.EnvironmentTurn);
    }

    public event EventHandler OnVictory;
    public event EventHandler OnDefeat;
    private void ShowEndGameScreen(bool victory)
    {
        InputManager.Instance.DisablePlayerInputAction();
        if (!NetworkManager.Singleton.IsServer)
        {
            EventSystem.current.sendNavigationEvents = false;
        }
        
        if (victory)
        {
            SoundFXManager.instance.PlaySoundFXCLip(AudioFiles.Instance.getVictoryAudio(), transform, 1f);
            OnVictory?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            SoundFXManager.instance.PlaySoundFXCLip(AudioFiles.Instance.getGameOverAudio(), transform, 1f);
            OnDefeat?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool AllRoundsAreDone()
    {
        return currentRoundNumber == TotalRounds;
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
    
    private void SetAmuletFieldsToGameFields()
    {
        if (PlayerAmuletSelection == null)
        {
            PlayerAmuletSelection = defaultAmulet;
        }
        
        TowerDefenseManager.TacticalPauseDuration = amuletSO.turnTime + PlayerAmuletSelection.turnTime;
        TowerDefenseManager.TotalRounds = amuletSO.numberOfTurns + PlayerAmuletSelection.numberOfTurns;;

        EnvironmentTurnManager.Instance.Turn = amuletSO.startingTurn + PlayerAmuletSelection.startingTurn;
        
        Ressource.SpawnRate = amuletSO.ressourceSpawnRate + PlayerAmuletSelection.ressourceSpawnRate;
        
        Player.Energy = amuletSO.playerEnergy + PlayerAmuletSelection.playerEnergy;
        Player.Health = amuletSO.playersHealth + PlayerAmuletSelection.playersHealth;

        EnemySpawnerManager.TotalRounds = amuletSO.numberOfTurns + PlayerAmuletSelection.numberOfTurns;;
        EnemySpawnerManager.timeBetweenSpawns = amuletSO.timeBetweenSpawns + PlayerAmuletSelection.timeBetweenSpawns;
        Enemy.Energy = amuletSO.enemyEnergy + PlayerAmuletSelection.enemyEnergy;
        
        GoofyEnemy.GoofyHealth = amuletSO.GoofyHealthPoints + PlayerAmuletSelection.GoofyHealthPoints;
        GoofyEnemy.GoofyMoveRation = amuletSO.GoofyMoveRatio + PlayerAmuletSelection.GoofyMoveRatio;

        PetiteMerdeEnemy.MerdeHealth = amuletSO.MerdeHeathPoints + PlayerAmuletSelection.MerdeHeathPoints;
        PetiteMerdeEnemy.MerdeMoveRatio = amuletSO.MerdeMoveRatio + PlayerAmuletSelection.MerdeMoveRatio;

        BigGuyEnemy.BigGuyAttack = amuletSO.BigGuyDamages + PlayerAmuletSelection.BigGuyDamages;
        BigGuyEnemy.BigGuyHealth = amuletSO.BigGuyHealthPoints + PlayerAmuletSelection.BigGuyHealthPoints;
        BigGuyEnemy.BigGuyMoveRatio = amuletSO.BigGuyMoveRatio + PlayerAmuletSelection.BigGuyMoveRatio;

        SniperEyeEnemy.SniperRange = amuletSO.SniperRange + PlayerAmuletSelection.SniperRange;
        SniperEyeEnemy.SniperMoveRatio = amuletSO.SniperMoveRatio + PlayerAmuletSelection.SniperMoveRatio;
        SniperEyeEnemy.SniperAttack = amuletSO.SniperDamages + PlayerAmuletSelection.SniperDamages;
        SniperEyeEnemy.SniperHealth = amuletSO.SniperHealthPoints + PlayerAmuletSelection.SniperHealthPoints;

        Obstacle.ObstacleHealth = amuletSO.ObstaclesHealth + PlayerAmuletSelection.ObstaclesHealth;
        CentralizedInventory.StartingMoney = amuletSO.startingMoney + PlayerAmuletSelection.startingMoney;

        BasicTrap.SetCost = amuletSO.TrapCost + PlayerAmuletSelection.TrapCost;
        BasicTrap.StunDuration = amuletSO.StunDuration + PlayerAmuletSelection.StunDuration;
        BasicTrap.TrapRange = amuletSO.TrapRange + PlayerAmuletSelection.TrapRange;

        ZombotTrap.SetCost = amuletSO.BombCost + PlayerAmuletSelection.BombCost;
        ZombotTrap.Damage = amuletSO.BombDamage + PlayerAmuletSelection.BombDamage;
        ZombotTrap.BombRange = amuletSO.BombRange + PlayerAmuletSelection.BombRange;

        BasicTower.BasicTowerProjectilesNumber = amuletSO.numberOfProjectile + PlayerAmuletSelection.numberOfProjectile;
        BasicTower.BasicTowerCost = amuletSO.TowerCost + PlayerAmuletSelection.TowerCost;
        BasicTower.BasicTowerRange = amuletSO.TowerRange + PlayerAmuletSelection.TowerRange;
        BasicTower.BasicTowerHealth = amuletSO.TowerHealth + PlayerAmuletSelection.TowerHealth;
        BasicTower.BasicTowerTimeBetweenShots = amuletSO.TowerTimeBetweenAttacks + PlayerAmuletSelection.TowerTimeBetweenAttacks;
        BasicTower.BasicTowerDamage = amuletSO.TowerDamage + PlayerAmuletSelection.TowerDamage;
        
        SynchronizeBuilding.Instance.OverrideBuildingCosts();
    }
    
    public event EventHandler<OnPlayerHealthChangedEventArgs> OnPlayerHealthChanged;
    public class OnPlayerHealthChangedEventArgs : EventArgs
    {
        public int HealthValue;
    }
    
    [ClientRpc()]
    public void EmitOnPlayerHealthChangedClientRpc(int changedHealth)
    {
        OnPlayerHealthChanged?.Invoke(this, new OnPlayerHealthChangedEventArgs
        {
            HealthValue = changedHealth
        });
    }

    public static void ResetStaticData()
    {
        if (SceneManager.GetActiveScene().name == Loader.Scene.MainMenuScene.ToString())
        {
            PlayerAmuletSelection = null;
        }
        
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

    
    public void AddMalus(GameObject malus)
    {
        this.bonuses.Add(malus);
    }

    public void RemoveMalus(GameObject malus)
    {
        this.maluses.Remove(malus);
    }

    private void CleanBonuses()
    {
        foreach (var bonus in bonuses)
        {
            Destroy(bonus);
        }
    }


    private void CleanMaluses()
    {
        foreach (var malus in maluses)
        {
            Destroy(malus);
        }
    }
    public static void ResetPlayerAmuletSelection()
    {
        PlayerAmuletSelection = null;
    }
        
    public override void OnDestroy()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= NetworkManager_OnLoadEventCompleted;
        base.OnDestroy();
    }
    
}
