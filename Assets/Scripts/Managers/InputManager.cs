using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
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
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Select.performed += Select;
    }

    private void Update()
    {
        Vector2 input = _playerInputActions.Player.Movement.ReadValue<Vector2>();

        _player.Move(input);
    }

    private void Select(InputAction.CallbackContext context)
    {
        _player.OnSelect(context);
    }

    private bool IsScene(String name)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        return currentScene.name == name; 
    }
}
