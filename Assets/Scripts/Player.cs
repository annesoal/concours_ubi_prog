using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{

    [SerializeField] private GameObject playerVisuals;
    [Header("Player Movement")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerGravity;
    [SerializeField] private float playerJumpForce;
    
    private Rigidbody _playerRigidBody;
    private Collider _playerCollider; 

    private void Awake()
    {
         _playerRigidBody = GetComponent<Rigidbody>();
         _playerCollider = playerVisuals.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        HandleHorizontalMovement();

        HandleJump();

        ApplyPlayerGravity();
    }

    private void HandleHorizontalMovement()
    {
        Vector2 movementNormalized = GameInput.Instance.GetMovementNormalized();

        Vector3 movementToApply = new Vector3(movementNormalized.x, 0, movementNormalized.y);

        transform.position += movementToApply * (Time.deltaTime * playerSpeed);
    }

    private void HandleJump()
    {
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            Vector3 vector3 = _playerRigidBody.velocity;
            vector3.y = playerJumpForce;
            _playerRigidBody.velocity = vector3;
        }
    }

    private bool IsGrounded()
    {
        Vector3 position = transform.position;
        
        Debug.Log("isGrounded : " + Physics.Raycast(position, new Vector3(0, -1, 0), 0.1f));
        
        return Physics.Raycast(_playerCollider.bounds.center, Vector3.down, 1.5f);
    }

    private void ApplyPlayerGravity()
    {
        _playerRigidBody.AddForce(0, playerGravity, 0);
    }
}
