using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;
    
    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        
        _playerInputActions.Player.Enable();
    }

    private void OnDestroy()
    {
        _playerInputActions.Dispose();
    }

    public Vector2 GetMovementNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
        
}
