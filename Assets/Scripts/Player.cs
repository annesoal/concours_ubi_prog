using System;
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
    [SerializeField] private Character _character;
    [SerializeField] private TileSelector _selector;

    private float _timer;

    private void Awake()
    {
        direction.Enable();
        selectorActivator.Enable();
    }

    private void Start()
    {
        _timer = cooldown;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_canSelectNextTile)
        {
            // TODO : Utiliser autre systeme d'input
            if (selectorActivator.WasPerformedThisFrame())
            {
                _canSelectNextTile = false;
                _selector.Initialize(_character.transform.position);
            }
        }
        else
        {
            var vector = GetDirectionInput();
            _selector.Control(vector, selectorActivator.WasPerformedThisFrame());
            _timer = cooldown;
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

    // Permet de pas controller les autres "players" 
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
    }
}