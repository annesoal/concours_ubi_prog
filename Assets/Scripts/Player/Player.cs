using Grid;
using Grid.Blocks;
using Unity.Mathematics;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;

public class Player : NetworkBehaviour
{
    public static Player LocalInstance { get; private set; }

    private const float MinPressure = 0.3f; 
    private const string SPAWN_POINT_COMPONENT_ERROR =
        "Chaque spawn point de joueur doit avoir le component `BlockPlayerSpawn`";
    [SerializeField] private float cooldown = 0.1f;
    [SerializeField] private PlayerTileSelector _selector;
    [SerializeField] private GameObject _highlighter;
    
    private Recorder<GameObject> _highlighters;
    private Timer _timer;

    public int EnergyAvailable
    {
        set
        {
            _totalEnergy = value;
            _currentEnergy = value;
        }
    }

    private bool _canMove;

    private int _totalEnergy;
    private int _currentEnergy;
    
    public Player()
    {
        _timer = new(cooldown);
        _highlighters = new();
    }
    public void Move(Vector2 direction)
    {
        if (IsMovementInvalid()) return;
        if (direction == Vector2.zero) return;
        if (_canMove)
            HandleInput(direction);
    }

    /// <summary>
    /// Check si les conditions de deplacement sont valides.
    /// </summary>
    /// <returns> true si elles sont invalides</returns>
    private bool IsMovementInvalid()
    {
        if (!IsOwner) return true;
        if (!CooldownHasPassed()) return true;
        return !HasEnergy();
    }

    private void HandleInput(Vector2 direction)
    {
        Vector2Int input = TranslateToVector2Int(direction);
        var savedSelectorPosition = SaveSelectorPosition();
        bool hasMoved = _selector.MoveSelector(input);

        if (hasMoved)
        {
            DecrementEnergy(input);
            AddHighlighter(savedSelectorPosition);
            _timer.Start();
        }
    }

    public Vector3 SaveSelectorPosition()
    {
        return _selector.transform.position;
    }

    private void AddHighlighter(Vector3 position)
    {
        GameObject newHighlighter = Instantiate(_highlighter, position, quaternion.identity);
        _highlighters.Add(newHighlighter);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
            InputManager.Player = this;
        }
        
        CharacterSelectUI.CharacterId characterSelection =
            GameMultiplayerManager.Instance.GetCharacterSelectionFromClientId(OwnerClientId);

        if (characterSelection == CharacterSelectUI.CharacterId.Monkey)
        {
            MovePlayerOnSpawnPoint(TowerDefenseManager.Instance.MonkeyBlockPlayerSpawn);
        }
        else
        {
            MovePlayerOnSpawnPoint(TowerDefenseManager.Instance.RobotBlockPlayerSpawn);
        }
    }
    private void MovePlayerOnSpawnPoint(Transform spawnPoint)
    {
        bool hasComponent = spawnPoint.TryGetComponent(out BlockPlayerSpawn blockPlayerSpawn);
        
        if (hasComponent)
        {
            blockPlayerSpawn.SetPlayerOnBlock(transform);
        }
        else
        {
            Debug.LogError(SPAWN_POINT_COMPONENT_ERROR);
        }
    }
    /// <summary>
    /// Demande au timer de verifier si le temps ecoule permet un nouveau deplacement
    /// </summary>
    /// <returns></returns>
    private bool CooldownHasPassed()
    {
        return _timer.HasTimePassed();
    }
    
    /// <summary>
    ///  Traduite la valeur d'input en Vector2Int
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private Vector2Int TranslateToVector2Int(Vector2 value)
    {
        Vector2Int translation = new Vector2Int();
        // pas tres jolie mais franchement ca marche 
        if (value.x > MinPressure)
        {
            translation.x = 1;
        }

        if (value.x < -MinPressure)
        {
            translation.x = -1;
        }

        if (value.y > MinPressure)
        {
            translation.y = +1;
        }

        if (value.y < -MinPressure)
        {
            translation.y = -1;
        }

        return translation;
    }

    public void OnConfirm()
    {
        TowerDefenseManager.Instance.SetPlayerReadyToPass(true);
        _canMove = false;
        _selector.Confirm();
    }

    // Methode appellee que le joeur appuie sur le bouton de selection (A sur gamepad par defaut ou spece au clavier)
    public void OnSelect()
    {
        if (_selector.isSelecting)
            return;
        
        _selector.Select();
        _selector.isSelecting = true;
        _selector.Initialize(transform.position); 
        TowerDefenseManager.Instance.SetPlayerReadyToPass(false);
        _canMove = true; 
    }

    public void Move()
    {
        _selector.Disable(); 
        Vector2Int? nextPosition = _selector.GetNextPositionToGo();
        if (nextPosition == null) return;

        RemoveNextHighlighter();
        MoveToNextPosition((Vector2Int) nextPosition); 
    }

    private void CleanHighlighters()
    {
        while (_highlighters != null && !_highlighters.IsEmpty())
        {
            RemoveNextHighlighter();
        }
    }
    private void RemoveNextHighlighter()
    {
        if (!_highlighters.IsEmpty())
        {
            GameObject nextHighLighter = _highlighters.RemoveLast();
            Destroy(nextHighLighter);
        }
    }
    private bool HasEnergy()
    {
        return _currentEnergy > 0; 
    }

    private void ResetEnergy()
    {
        _currentEnergy = _totalEnergy;
    }

    private void DecrementEnergy(Vector2Int input)
    {
       _currentEnergy--; 
    }

    public void OnCancel()
    {
        ResetEnergy();
        CleanHighlighters();
        _selector.ResetSelf();
        TowerDefenseManager.Instance.SetPlayerReadyToPass(false);
    }

    private void MoveToNextPosition(Vector2Int toPosition)
    {
        Vector3 cellLocalPosition = TilingGrid.GridPositionToLocal(toPosition);
        transform.LookAt(cellLocalPosition);
        transform.position = cellLocalPosition;
    }

    public void ResetPlayer(int energy)
    {
        EnergyAvailable = energy;
        _canMove = false;
        _selector.ResetSelf();
        _highlighters.Reset();
    }

    public void PrepareToMove()
    {
        _selector.GetNextPositionToGo();
    }
}
