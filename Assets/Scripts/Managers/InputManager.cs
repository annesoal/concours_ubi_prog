using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    private PlayerInputActions _playerInputActions;

    private static Player _player;
    public static Player Player
    {
        set
        {
            if (value.IsOwner)
                _player = value;
        }
    } 

    private void Awake()
    {
        Instance = this;
        
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Select.performed += Select;
        _playerInputActions.Player.Cancel.performed += Cancel;
        _playerInputActions.Player.Confirm.performed += Confirm;
    }

    private void Confirm(InputAction.CallbackContext obj)
    {
        _player.OnConfirm();
    }

    private void Cancel(InputAction.CallbackContext obj)
    {
        _player.OnCancel();
    }

    private void Update()
    {
        if (_player == null) return; 
        Vector2 input = _playerInputActions.Player.Movement.ReadValue<Vector2>();

        _player.Move(input);
    }

    public Vector2 GetCameraMoveInput()
    {
        Vector2 cameraInputVector = _playerInputActions.Player.CameraMove.ReadValue<Vector2>();

        return cameraInputVector;
    }

    public float GetCameraZoomInput()
    {
        return _playerInputActions.Player.CameraZoom.ReadValue<float>();
    }

    public float GetCameraRotationInput()
    {
        return _playerInputActions.Player.CameraRotation.ReadValue<float>();
    }

    private void Select(InputAction.CallbackContext context)
    {
        _player.OnSelect();
    }
}
