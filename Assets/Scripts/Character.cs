using Unity.Netcode;
using UnityEngine;

public class Character : NetworkBehaviour
{
    [SerializeField] private GameObject playerVisuals;

    private Collider _playerCollider;
    private Rigidbody _playerRigidBody;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _playerCollider = playerVisuals.GetComponent<CapsuleCollider>();
    }
    
}