using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    private PlayerInputActions _playerInputActions;

    private static Player _player;
    private bool _canMovePlayer = false; 
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
        _playerInputActions.Camera.Enable();
        _playerInputActions.Player.Select.performed += Select;
        _playerInputActions.Player.Cancel.performed += Cancel;
        _playerInputActions.Player.Confirm.performed += Confirm;
    }


    private void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += UpdateCanPlayState;
    }

    private void UpdateCanPlayState(object o, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == TowerDefenseManager.State.TacticalPause)
        {
            _canMovePlayer = true;
        }
        else 
        {
            _canMovePlayer = false;
        }
    }

    private void Confirm(InputAction.CallbackContext obj)
    {
        if(_canMovePlayer)
            _player.OnConfirm();
        
    }

    private void Cancel(InputAction.CallbackContext obj)
    {
        if(_canMovePlayer)
            _player.OnCancel();
    }

    private void Update()
    {
        if (_player == null) return; 

        if(_canMovePlayer){
            Vector2 input = _playerInputActions.Player.Movement.ReadValue<Vector2>();
            _player.InputMove(input);
        }
    }

    public Vector2 GetCameraMoveInput()
    {
        Vector2 cameraInputVector = _playerInputActions.Camera.CameraMove.ReadValue<Vector2>();

        return cameraInputVector;
    }

    public float GetCameraZoomInput()
    {
        return _playerInputActions.Camera.CameraZoom.ReadValue<float>();
    }

    public float GetCameraRotationInput()
    {
        return _playerInputActions.Camera.CameraRotation.ReadValue<float>();
    }

    public void DisablePlayerInputMap()
    {
        _playerInputActions.Player.Disable();
    }
    
    public void EnablePlayerInputMap()
    {
        _playerInputActions.Player.Enable();
    }

    private void Select(InputAction.CallbackContext context)
    {
        if(_canMovePlayer)
            _player.OnSelect();
    }
}
