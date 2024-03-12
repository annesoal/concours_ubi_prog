using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    private bool _isInTacticalPausePhase; 

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Select.performed += Select;
        _playerInputActions.Player.Cancel.performed += Reset;
    }

    private void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += ChangeIsTacticalPausePhase;
    }

    private void Update()
    {
        if (Player.Instance == null) return;
        if (_isInTacticalPausePhase)
        {
            Vector2 input = _playerInputActions.Player.Movement.ReadValue<Vector2>();
            Player.Instance.MoveSelector(input);
        }
    }

    private void Select(InputAction.CallbackContext context)
    {
        if (_isInTacticalPausePhase)
            Player.Instance.OnSelect(context);
    }

    private void Reset(InputAction.CallbackContext context)
    {
        if (_isInTacticalPausePhase)
            Player.Instance.OnCancel(context);
    }

    private void ChangeIsTacticalPausePhase(object sender, 
        TowerDefenseManager.OnCurrentStateChangedEventArgs changedEventArgs)
    {
        if (changedEventArgs.newValue == TowerDefenseManager.State.TacticalPause)
        {
            this._isInTacticalPausePhase = true;
            Player.Instance.Energy = TowerDefenseManager.Instance.EnergyAvailable;
        }
        else if (changedEventArgs.newValue == TowerDefenseManager.State.EnvironmentTurn)
            this._isInTacticalPausePhase = false; 
    }
}
