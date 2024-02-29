using System;
using Grid.Blocks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    private const float MinPressure = 0.3f;
    public InputAction direction;

    public InputAction selectorActivator;

    [SerializeField] private float cooldown = 0.1f;

    
    public bool CanSelectNextTile
    {
        set => _canSelectNextTile = value;
    }

    private bool _canSelectNextTile = true;
    [SerializeField] private TileSelector _selector;

    private float _timer;
    
    private const string SPAWN_POINT_COMPONENT_ERROR =
        "Chaque spawn point de joueur doit avoir le component `BlockPlayerSpawn`";

    private void Awake()
    {
        direction.Enable();
        selectorActivator.Enable();
    }

    private void Start()
    {
        _timer = cooldown;
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

    // Update is called once per frame
    private void Update()
    {
        if (!IsOwner) { return; }

        if (_canSelectNextTile)
        {
            // TODO : Utiliser autre systeme d'input
            if (selectorActivator.WasPerformedThisFrame())
            {
                _canSelectNextTile = false;
                _selector.Initialize(transform.position);
            }
        }
        else
        {
            var vector = GetDirectionInput();
            _selector.Control(vector, selectorActivator.WasPerformedThisFrame());
            _timer = cooldown;
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

    private Vector2Int GetDirectionInput()
    {
        var input = direction.ReadValue<Vector2>();
        Vector2Int translation = new Vector2Int();
        if (input.x > MinPressure)
        {
            translation.x = 1; 
        }

        if (input.x < -MinPressure)
        {
            translation.x = -1; 
        }

        if (input.y > MinPressure)
        {
            translation.y = +1; 
        }

        if (input.y < -MinPressure)
        {
            translation.y = -1;
        }
        return translation;
    }
}
