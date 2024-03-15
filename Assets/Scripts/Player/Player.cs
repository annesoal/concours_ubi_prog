using Grid;
using Grid.Blocks;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    public static Player LocalInstance { get; private set; }

    private const float MinPressure = 0.3f; 
    private const string SPAWN_POINT_COMPONENT_ERROR =
        "Chaque spawn point de joueur doit avoir le component `BlockPlayerSpawn`";
    [SerializeField] private float cooldown = 0.1f;
    [SerializeField] private PlayerTileSelector _selector;
    private Timer _timer;

    public int EnergyAvailable
    {
        set
        {
            _totalEnergy = value;
            _currentEnergy = value;
        }
    }

    private int _totalEnergy;
    private int _currentEnergy;
    
    public Player()
    {
        _timer = new(cooldown);
    }
    public void Move(Vector2 direction)
    {
        if (IsMovementInvalid()) return;
        HandleInput(direction);
    }

    /// <summary>
    /// Check si les conditions de deplacement sont valides.
    /// </summary>
    /// <returns> true si elles sont invalides</returns>
    private bool IsMovementInvalid()
    {
        if (!IsOwner) return true;
        if (!CanMove()) return true;
        return !HasEnergy();
    }

    private void HandleInput(Vector2 direction)
    {
        Vector2Int input = TranslateToVector2Int(direction);
        DecrementEnergy(input);
        _selector.MoveSelector(input);
        _timer.Start();
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
    private bool CanMove()
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
    }

    // Methode appellee que le joeur appuie sur le bouton de selection (A sur gamepad par defaut ou spece au clavier)
    public void OnSelect()
    {
        if (_selector.isSelecting)
            return;
        
        _selector.isSelecting = true;
        _selector.Initialize(transform.position); 
        TowerDefenseManager.Instance.SetPlayerReadyToPass(false);
    }

    public void Move()
    {
        _selector.Disable(); 
        Vector2Int? nextPosition = _selector.GetNextPositionToGo();
        if (nextPosition == null) return;
        
        MoveToNextPosition((Vector2Int) nextPosition); 
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
        if (input != Vector2Int.zero)
        {
            _currentEnergy--; 
        }
    }

    public void OnCancel()
    {
        ResetEnergy();
        _selector.ResetSelf();
        TowerDefenseManager.Instance.SetPlayerReadyToPass(false);
    }

    private void MoveToNextPosition(Vector2Int toPosition)
    {
        Vector3 cellLocalPosition = TilingGrid.GridPositionToLocal(toPosition);
        transform.LookAt(cellLocalPosition);
        transform.position = cellLocalPosition;
    }
}
