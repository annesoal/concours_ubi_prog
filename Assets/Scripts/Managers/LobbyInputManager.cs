using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyInputManager : MonoBehaviour
{
    public static LobbyInputManager Instance { get; private set; }
        
    private PlayerInputActions _playerInputActions;
    
    private void Awake()
    {
        Instance = this; 
        
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.UI.Enable();
        
        _playerInputActions.UI.Select.performed += PlayerInputActionUI_OnSelectperformed;
        _playerInputActions.UI.Cancel.performed += PlayerInputActionUI_OnCancelperformed;
        _playerInputActions.UI.MinimalUp.performed += PlayerInputActionUI_OnUpPerformed;
        _playerInputActions.UI.MinimalDown.performed += PlayerInputActionUI_OnDownPerformed;
    }

    public event EventHandler OnSelectUIPressed;
    private void PlayerInputActionUI_OnSelectperformed(InputAction.CallbackContext obj)
    {
        OnSelectUIPressed?.Invoke(this, EventArgs.Empty);
    }
    
    public event EventHandler OnCancelUIPressed;
    private void PlayerInputActionUI_OnCancelperformed(InputAction.CallbackContext obj)
    {
        OnCancelUIPressed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnUpUIPressed;
    private void PlayerInputActionUI_OnUpPerformed(InputAction.CallbackContext obj)
    {
        OnUpUIPressed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnDownUIPressed;
    private void PlayerInputActionUI_OnDownPerformed(InputAction.CallbackContext obj)
    {
        OnDownUIPressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        _playerInputActions.UI.Select.performed -= PlayerInputActionUI_OnSelectperformed;
        _playerInputActions.UI.Cancel.performed -= PlayerInputActionUI_OnCancelperformed;
        _playerInputActions.UI.MinimalUp.performed -= PlayerInputActionUI_OnUpPerformed;
        _playerInputActions.UI.MinimalDown.performed -= PlayerInputActionUI_OnDownPerformed;
        
        _playerInputActions.Dispose();
    }
}
