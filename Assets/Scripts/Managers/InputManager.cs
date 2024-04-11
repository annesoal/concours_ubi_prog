using System;
using System.Collections.Generic;
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
        
		LoadSavedBindings();
        
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Select.performed += Select;
        _playerInputActions.Player.Cancel.performed += Cancel;
        _playerInputActions.Player.Confirm.performed += Confirm;
        _playerInputActions.Player.Interact.performed += PlayerInput_OnInteractperformed;
        
        _playerInputActions.UI.Select.performed += UserInterfaceInput_OnSelectperformed;
        _playerInputActions.UI.Cancel.performed += UserInterfaceInput_OnCancelPerformed;
        _playerInputActions.UI.Left.performed += UserInterfaceInput_OnLeftPerformed ;
        _playerInputActions.UI.Right.performed += UserInterfaceInput_OnRightPerformed;
        _playerInputActions.UI.Up.performed += UserInterfaceInput_OnUpPerformed;
        _playerInputActions.UI.Down.performed += UserInterfaceInput_OnDownPerformed;
        
        _playerInputActions.UI.MinimalLeft.performed += UserInterfaceInput_OnMinimalLeftPerformed;
        _playerInputActions.UI.MinimalRight.performed += UserInterfaceInput_OnMinimalRightPerformed;
        
        _playerInputActions.UI.ShoulderRight.performed += UserInterfaceInput_OnShoulderRightPerformed;
        _playerInputActions.UI.ShoulderLeft.performed += UserInterfaceInput_OnShoulderLeftPerformed;
        
		InitializeActionMapBindingEquivalent();
    }

    private void Start()
    {
	    if (TowerDefenseManager.Instance != null)
	    {
		    TowerDefenseManager.Instance.OnCurrentStateChanged += UpdateCanPlayState;
	    }
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
    
    public void DisablePlayerInputAction()
    {
        _playerInputActions.Disable();
    }

    private void Confirm(InputAction.CallbackContext obj)
    {
        if(_canMovePlayer && _player != null)
            _player.OnConfirm();
        
    }

    private void Cancel(InputAction.CallbackContext obj)
    {
        if(_canMovePlayer && _player != null)
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
    
    public void EnableUserInterfaceInputMap()
    {
        _playerInputActions.UI.Enable();
    }
    
    public void DisableUserInterfaceInputMap()
    {
        _playerInputActions.UI.Disable();
    }

    private void Select(InputAction.CallbackContext context)
    {
	    if (_canMovePlayer && _player != null)
            _player.OnSelect();
    }
    
    public event EventHandler OnPlayerInteractPerformed;
    private void PlayerInput_OnInteractperformed(InputAction.CallbackContext obj)
    {
        OnPlayerInteractPerformed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnUserInterfaceSelectPerformed;
    private void UserInterfaceInput_OnSelectperformed(InputAction.CallbackContext obj)
    {
        OnUserInterfaceSelectPerformed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnUserInterfaceCancelPerformed;
    private void UserInterfaceInput_OnCancelPerformed(InputAction.CallbackContext obj)
    {
        OnUserInterfaceCancelPerformed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnUserInterfaceLeftPerformed;
    private void UserInterfaceInput_OnLeftPerformed(InputAction.CallbackContext obj)
    {
        OnUserInterfaceLeftPerformed?.Invoke(this, EventArgs.Empty);
    }
    
    public event EventHandler OnUserInterfaceRightPerformed;
    private void UserInterfaceInput_OnRightPerformed(InputAction.CallbackContext obj)
    {
        OnUserInterfaceRightPerformed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnUserInterfaceUpPerformed;
    private void UserInterfaceInput_OnUpPerformed(InputAction.CallbackContext obj)
    {
        OnUserInterfaceUpPerformed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnUserInterfaceDownPerformed;
    private void UserInterfaceInput_OnDownPerformed(InputAction.CallbackContext obj)
    {
        OnUserInterfaceDownPerformed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnUserInterfaceMinimalLeftPerformed;
    private void UserInterfaceInput_OnMinimalLeftPerformed(InputAction.CallbackContext obj)
    {
        OnUserInterfaceMinimalLeftPerformed?.Invoke(this, EventArgs.Empty);
    }
    
    public event EventHandler OnUserInterfaceMinimalRightPerformed;
    private void UserInterfaceInput_OnMinimalRightPerformed(InputAction.CallbackContext obj)
    {
        OnUserInterfaceMinimalRightPerformed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnUserInterfaceShoulderRightPerformed;
    private void UserInterfaceInput_OnShoulderRightPerformed(InputAction.CallbackContext obj)
    {
        OnUserInterfaceShoulderRightPerformed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnUserInterfaceShoulderLeftPerformed;
    private void UserInterfaceInput_OnShoulderLeftPerformed(InputAction.CallbackContext obj)
    {
        OnUserInterfaceShoulderLeftPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Select.performed -= Select;
        _playerInputActions.Player.Cancel.performed -= Cancel;
        _playerInputActions.Player.Confirm.performed -= Confirm;
        _playerInputActions.Player.Interact.performed -= PlayerInput_OnInteractperformed;
        
        _playerInputActions.UI.Select.performed -= UserInterfaceInput_OnSelectperformed;
        _playerInputActions.UI.Cancel.performed -= UserInterfaceInput_OnCancelPerformed;
        _playerInputActions.UI.Left.performed -= UserInterfaceInput_OnLeftPerformed ;
        _playerInputActions.UI.Right.performed -= UserInterfaceInput_OnRightPerformed;
        _playerInputActions.UI.Up.performed -= UserInterfaceInput_OnUpPerformed;
        _playerInputActions.UI.Down.performed -= UserInterfaceInput_OnDownPerformed;
        
        _playerInputActions.UI.MinimalLeft.performed -= UserInterfaceInput_OnMinimalLeftPerformed;
        _playerInputActions.UI.MinimalRight.performed -= UserInterfaceInput_OnMinimalRightPerformed;
        
        _playerInputActions.UI.ShoulderRight.performed -= UserInterfaceInput_OnShoulderRightPerformed;
        _playerInputActions.UI.ShoulderLeft.performed -= UserInterfaceInput_OnShoulderLeftPerformed;
        
        _playerInputActions.Dispose();
    }
    
    // REBINDING
    
	public enum Binding
	{
		Up = 0,
		Down,
		Left,
		Right,
		Select,
		Cancel,
		Confirm,
		Interact,
		MinimalUp,
		MinimalDown,
		MinimalLeft,
		MinimalRight,
		ShoulderLeft,
		ShoulderRight,
	}
    
	private const int SINGLE_ACTION_MAP_DEFAULT_INDEX = 0;

	private const int UP_ACTION_MAP_INDEX = 1;
	private const int DOWN_ACTION_MAP_INDEX = 2;
	private const int LEFT_ACTION_MAP_INDEX = 3;
	private const int RIGHT_ACTION_MAP_INDEX = 4;
	
	private const int SELECT_ACTION_MAP_INDEX = 0;
	private const int CANCEL_ACTION_MAP_INDEX = 0;
	private const int CONFIRM_ACTION_MAP_INDEX = 0;
	private const int INTERACT_ACTION_MAP_INDEX = 0;
	
	private const int MINIMAL_UP_ACTION_MAP_INDEX = 0;
	private const int MINIMAL_DOWN_ACTION_MAP_INDEX = 0;
	private const int MINIMAL_LEFT_ACTION_MAP_INDEX = 0;
	private const int MINIMAL_RIGHT_ACTION_MAP_INDEX = 0;
	
	private const int SHOULDER_LEFT_ACTION_MAP_INDEX = 0;
	private const int SHOULDER_RIGHT_ACTION_MAP_INDEX = 0;

	private static readonly Dictionary<Binding, int> BindingActionMapIndexEquivalent = new Dictionary<Binding, int>()
	{
		{Binding.Up, UP_ACTION_MAP_INDEX},
		{Binding.Down, DOWN_ACTION_MAP_INDEX},
		{Binding.Left, LEFT_ACTION_MAP_INDEX},
		{Binding.Right, RIGHT_ACTION_MAP_INDEX},
		{Binding.Select, SELECT_ACTION_MAP_INDEX},
		{Binding.Cancel, CANCEL_ACTION_MAP_INDEX},
		{Binding.Confirm, CONFIRM_ACTION_MAP_INDEX},
		{Binding.Interact, INTERACT_ACTION_MAP_INDEX},
		{Binding.MinimalUp, MINIMAL_UP_ACTION_MAP_INDEX},
		{Binding.MinimalDown, MINIMAL_DOWN_ACTION_MAP_INDEX},
		{Binding.MinimalLeft, MINIMAL_LEFT_ACTION_MAP_INDEX},
		{Binding.MinimalRight, MINIMAL_RIGHT_ACTION_MAP_INDEX},
		{Binding.ShoulderLeft, SHOULDER_LEFT_ACTION_MAP_INDEX},
		{Binding.ShoulderRight, SHOULDER_RIGHT_ACTION_MAP_INDEX},
	};

	private Dictionary<Binding, InputAction> _actionMapBindingEquivalent;

	private void InitializeActionMapBindingEquivalent()
	{
		_actionMapBindingEquivalent = new Dictionary<Binding, InputAction>()
		{
			{ Binding.Up, _playerInputActions.Player.Movement },
			{ Binding.Down, _playerInputActions.Player.Movement },
			{ Binding.Left, _playerInputActions.Player.Movement },
			{ Binding.Right, _playerInputActions.Player.Movement },
			{ Binding.Select, _playerInputActions.Player.Select },
			{ Binding.Cancel, _playerInputActions.Player.Cancel },
			{ Binding.Confirm, _playerInputActions.Player.Confirm },
			{ Binding.Interact, _playerInputActions.Player.Interact },
			{ Binding.MinimalUp, _playerInputActions.UI.MinimalUp },
			{ Binding.MinimalDown, _playerInputActions.UI.MinimalUp },
			{ Binding.MinimalLeft, _playerInputActions.UI.MinimalUp },
			{ Binding.MinimalRight, _playerInputActions.UI.MinimalUp },
			{ Binding.ShoulderLeft, _playerInputActions.UI.ShoulderLeft },
			{ Binding.ShoulderRight, _playerInputActions.UI.ShoulderRight },
		};
	}

	public string GetBindingText(Binding binding)
	{
		InputAction actionMapOfBinding = _actionMapBindingEquivalent[binding]; 
		
		return actionMapOfBinding.bindings[BindingActionMapIndexEquivalent[binding]].ToDisplayString();
	}

	public event EventHandler OnInputRebindingCompleted;
	
	private const string BINDINGS_JSON_KEY = "BindingJsonKey";
	
	public void RebindBinding(Binding toRebind, Action onRebindDone)
	{
		_playerInputActions.Player.Disable();

		InputAction inputActionOfToRebind = _actionMapBindingEquivalent[toRebind];
		
		inputActionOfToRebind.PerformInteractiveRebinding(BindingActionMapIndexEquivalent[toRebind])
			.OnComplete(callback =>
			{
				Debug.Log(callback.action.bindings[BindingActionMapIndexEquivalent[toRebind]].path);
				Debug.Log(callback.action.bindings[BindingActionMapIndexEquivalent[toRebind]].overridePath);
				
				TryRebindUserInterfaceBinding(callback.action, toRebind);
				
				callback.Dispose();
				
				_playerInputActions.Enable();
				
				onRebindDone();

				//PlayerPrefs.SetString(BINDINGS_JSON_KEY, _playerInputActions.SaveBindingOverridesAsJson());
				//PlayerPrefs.Save();
				
				OnInputRebindingCompleted?.Invoke(this, EventArgs.Empty);
			})
			.Start();
	}

	private void TryRebindUserInterfaceBinding(InputAction inputActionRebinded, Binding playerEquivalentRebinded)
	{
		string path = inputActionRebinded.bindings[BindingActionMapIndexEquivalent[playerEquivalentRebinded]].path;
			
		if (inputActionRebinded == _actionMapBindingEquivalent[Binding.Up])
		{
			RebindUserInterfaceBinding(_playerInputActions.UI.Up, path);
			RebindUserInterfaceBinding(_playerInputActions.UI.MinimalUp, path);
			return;
		}
		
		if (inputActionRebinded == _actionMapBindingEquivalent[Binding.Down])
		{
			RebindUserInterfaceBinding(_playerInputActions.UI.Down, path);
			RebindUserInterfaceBinding(_playerInputActions.UI.MinimalDown, path);
			return;
		}
		
		if (inputActionRebinded == _actionMapBindingEquivalent[Binding.Left])
		{
			RebindUserInterfaceBinding(_playerInputActions.UI.Left, path);
			RebindUserInterfaceBinding(_playerInputActions.UI.MinimalLeft, path);
			return;
		}
		
		if (inputActionRebinded == _actionMapBindingEquivalent[Binding.Left])
		{
			RebindUserInterfaceBinding(_playerInputActions.UI.Right, path);
			RebindUserInterfaceBinding(_playerInputActions.UI.MinimalRight, path);
			return;
		}
		
		if (inputActionRebinded == _actionMapBindingEquivalent[Binding.Select])
		{
			RebindUserInterfaceBinding(_playerInputActions.UI.Select, path);
			return;
		}
		
		if (inputActionRebinded == _actionMapBindingEquivalent[Binding.Cancel])
		{
			RebindUserInterfaceBinding(_playerInputActions.UI.Cancel, path);
			return;
		}
	}

	private void RebindUserInterfaceBinding(InputAction toRebind, string path)
	{
			toRebind.ApplyBindingOverride (0, path);
	}

	private void LoadSavedBindings()
	{
		if (PlayerPrefs.HasKey(BINDINGS_JSON_KEY))
		{
			_playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(BINDINGS_JSON_KEY));
		}
	}
}
