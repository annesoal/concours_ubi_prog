using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

        InitializeSyncerServerInputMethods();
    }

    public event EventHandler<FromServerEventArgs> OnUpUI;
    public class FromServerEventArgs : EventArgs
    {
        public bool SyncrhonizedCall;
    }
    
    private void PlayerInputUI_OnUpPerformed(InputAction.CallbackContext obj)
    {
        OnUpUI?.Invoke(this, new FromServerEventArgs
        {
            SyncrhonizedCall = false
        });
    }
    
    public event EventHandler<FromServerEventArgs> OnDownUI;
    private void PlayerInputUI_OnDownPerformed(InputAction.CallbackContext obj)
    {
        OnDownUI?.Invoke(this, new FromServerEventArgs
        {
            SyncrhonizedCall = false,
        });
    }

    public event EventHandler<FromServerEventArgs> OnRightUI;

    private void PlayerInputUI_OnRightPerformed(InputAction.CallbackContext obj)
    {
        OnRightUI?.Invoke(this, new FromServerEventArgs
        {
            SyncrhonizedCall = false,
        });
    }

    public event EventHandler<FromServerEventArgs> OnLeftUI;

    private void PlayerInputUI_OnLeftPerformed(InputAction.CallbackContext obj)
    {
        if (gameObject.activeSelf && NetworkManager.Singleton.IsServer)
        {
            OnLeftUI?.Invoke(this, new FromServerEventArgs
            {
                SyncrhonizedCall = false,
            });
        }
    }

    public event EventHandler<FromServerEventArgs> OnSelectUI;
    private void PlayerInputUI_OnSelectPerformed(InputAction.CallbackContext obj)
    {
        OnSelectUI?.Invoke(this, new FromServerEventArgs
        {
            SyncrhonizedCall = false,
        });
    }

    private void OnDestroy()
    {
        _playerInputActions.UI.Up.performed -= PlayerInputUI_OnUpPerformed;
        _playerInputActions.UI.Down.performed -= PlayerInputUI_OnDownPerformed;
        _playerInputActions.UI.Right.performed -= PlayerInputUI_OnRightPerformed;
        _playerInputActions.UI.Left.performed -= PlayerInputUI_OnLeftPerformed;

        _playerInputActions.UI.Select.performed -= PlayerInputUI_OnSelectPerformed;
    }

    private List<Action> _syncServerInputMethods;
    
    private void InitializeSyncerServerInputMethods()
    {
        _syncServerInputMethods = new List<Action>
        {
            () => { OnUpUI?.Invoke(this, new FromServerEventArgs { SyncrhonizedCall = true, }); },
            () => { OnDownUI?.Invoke(this, new FromServerEventArgs { SyncrhonizedCall = true, }); },
            () => { OnRightUI?.Invoke(this, new FromServerEventArgs { SyncrhonizedCall = true}); },
            () => { OnLeftUI?.Invoke(this, new FromServerEventArgs { SyncrhonizedCall = true}); },
            () => { OnSelectUI?.Invoke(this, new FromServerEventArgs { SyncrhonizedCall = true}); },
        };
    }
    
    public void SyncServerInput(Input toSync)
    {
        _syncServerInputMethods[(int)toSync]();
    }

    public enum Input
    {
        Up = 0,
        Down,
        Right,
        Left,
        Select,
    }
}