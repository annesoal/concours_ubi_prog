using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private void Update()
    {
        HandleMovement();
    }

    [SerializeField] private float playerSpeed;
    
    private void HandleMovement()
    {
        Vector2 movementNormalized = GameInput.Instance.GetMovementNormalized();

        Vector3 movementToApply = new Vector3(movementNormalized.x, 0, movementNormalized.y);

        transform.position += movementToApply * (Time.deltaTime * playerSpeed);
    }
}
