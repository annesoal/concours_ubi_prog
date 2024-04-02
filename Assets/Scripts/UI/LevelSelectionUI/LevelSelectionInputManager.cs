using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSelectionInputManager : MonoBehaviour
{
    public static LevelSelectionInputManager Instance { get; private set; }

    private PlayerInputActions _playerInputActions;
    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        
        _playerInputActions.UI.Enable();
        
        _playerInputActions.UI.Up.performed += PlayerInputUI_OnUpPerformed;
        _playerInputActions.UI.Down.performed += PlayerInputUI_OnDownPerformed;
        _playerInputActions.UI.Right.performed += PlayerInputUI_OnRightPerformed;
        _playerInputActions.UI.Left.performed += PlayerInputUI_OnLeftPerformed;

        _playerInputActions.UI.Select.performed += PlayerInputUI_OnSelectPerformed;
    }

    public event EventHandler OnUpUI;
    private void PlayerInputUI_OnUpPerformed(InputAction.CallbackContext obj)
    {
        if (gameObject.activeSelf)
        {
            OnUpUI?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public event EventHandler OnDownUI;
    private void PlayerInputUI_OnDownPerformed(InputAction.CallbackContext obj)
    {
        if (gameObject.activeSelf)
        {
            OnDownUI?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler OnRightUI;

    private void PlayerInputUI_OnRightPerformed(InputAction.CallbackContext obj)
    {
        if (gameObject.activeSelf)
        {
            OnRightUI?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler OnLeftUI;

    private void PlayerInputUI_OnLeftPerformed(InputAction.CallbackContext obj)
    {
        if (gameObject.activeSelf)
        {
            OnLeftUI?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler OnSelectUI;
    private void PlayerInputUI_OnSelectPerformed(InputAction.CallbackContext obj)
    {
        if (gameObject.activeSelf)
        {
            OnSelectUI?.Invoke(this, EventArgs.Empty);
        }
    }
}