using System;
using System.Collections;
using Grid.Blocks;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    private const float MinPressure = 0.3f; 
    private const string SPAWN_POINT_COMPONENT_ERROR =
        "Chaque spawn point de joueur doit avoir le component `BlockPlayerSpawn`";
    [SerializeField] private float cooldown = 0.2f;
    [SerializeField] private TileSelector _selector;

    private PlayerInputActions _playerInputActions;
    private bool IsMovingSelector { get; set; }
    private Timer _timer;

    public Player()
    {
        _timer = new(cooldown);
    }

    void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Select.performed += OnSelect;
    }

    void Update()
    {
        Vector2 value = _playerInputActions.Player.Movement.ReadValue<Vector2>();
        //TODO : RAAAAAAPPPHHH ?? CA MARCHE ?? 
        if (!IsOwner) return;
        if (!IsMovingSelector) return;
        if (!CanMove()) return;
        Vector2Int input = Translate(value);
        _selector.MoveSelector(input);
        _timer.Start();     
       
    }
    public override void OnNetworkSpawn()
    {
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
    private bool CanMove()
    {
        return _timer.HasTimePassed();
    }
    
    // Traduite la valeur d'input en Vector2Int
    private Vector2Int Translate(Vector2 value)
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

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (IsMovingSelector)
        {
            _selector.Destroy();
            IsMovingSelector = false;
        }
        else
        {
            _selector.Initialize(transform.position); 
            IsMovingSelector = true; 
        }
    }
    
}
