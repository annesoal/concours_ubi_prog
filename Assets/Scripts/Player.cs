using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private GameObject playerVisuals;
    [SerializeField] private TileSelector tileSelector;

    public bool CanSelectectNextTile
    {
        set => _canSelectNextTile = value;
    }
    
    private bool _canSelectNextTile = true;
    private Collider _playerCollider;
    private Rigidbody _playerRigidBody;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _playerCollider = playerVisuals.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (_canSelectNextTile)
        {
            // TODO : Utiliser autre systeme d'input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _canSelectNextTile = false;
                tileSelector.Initialize(transform.position);
            }
        }
        else
        {
            tileSelector.Control();
        }
    }
}