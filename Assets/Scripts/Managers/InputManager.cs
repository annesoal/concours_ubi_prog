using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; 

public class InputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public static Player player; 

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Select.performed += Select;
    }

    private void Update()
    {
        Vector2 input = _playerInputActions.Player.Movement.ReadValue<Vector2>();
        if (IsScene("Lobby"))
        {
            
        }
        else if (IsScene("Blocks"))
        {
            
        }
    }

    private void Select(InputAction.CallbackContext context)
    {
        if (IsScene("Lobby"))
        {
            
        }
        else if (IsScene("Blocks"))
        {
            
        }
    }

    private bool IsScene(String name)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        return currentScene.name == name; 
    }

    /**
     * Cette methode doit etre appellee par le joueur quand il est construit.
     */
    public static void SetPlayer(Player player)
    {
        InputManager.player = player;
    }
}
