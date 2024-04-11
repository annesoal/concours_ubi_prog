using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldControllerInput : MonoBehaviour
{
    [SerializeField] private Selectable toSelectAfterDeactivation;
    
    private bool _isControllerInputActive;

    private void Start()
    {
        InputManager.Instance.OnUserInterfaceSelectPerformed += LobbyInputManager_OnSelectUIPressed;
        InputManager.Instance.OnUserInterfaceCancelPerformed += LobbyInputManager_OnCancelUIPressed;
        InputManager.Instance.OnUserInterfaceUpPerformed += LobbyInputManager_OnUpUIPressed;
        InputManager.Instance.OnUserInterfaceDownPerformed += LobbyInputManager_OnDownUIPressed;
    }

    void Update()
    {
        _isControllerInputActive = EventSystem.current.currentSelectedGameObject == gameObject;

        if (_isControllerInputActive)
        {
            EventSystem.current.sendNavigationEvents = false;
        }
    }
    
    private void LobbyInputManager_OnSelectUIPressed(object sender, EventArgs e)
    {
        if (_isControllerInputActive)
        {
            // TODO show virtual keyboard
        }
    }
    
    private void LobbyInputManager_OnCancelUIPressed(object sender, EventArgs e)
    {
        if (_isControllerInputActive)
        {
            EventSystem.current.SetSelectedGameObject(toSelectAfterDeactivation.gameObject);
            
            StartCoroutine(ActivateNavigationEventAfterTime(TIMER_ACTIVATE_NAV_EVENT));
        }
    }

    private const float TIMER_ACTIVATE_NAV_EVENT = 0.08f;
    
    private void LobbyInputManager_OnUpUIPressed(object sender, EventArgs e)
    {
        if (_isControllerInputActive)
        {
            EventSystem.current.SetSelectedGameObject(
                gameObject.GetComponent<TMP_InputField>().navigation.selectOnUp.gameObject
            );

            StartCoroutine(ActivateNavigationEventAfterTime(TIMER_ACTIVATE_NAV_EVENT));
        }
    }

    private void LobbyInputManager_OnDownUIPressed(object sender, EventArgs e)
    {
        if (_isControllerInputActive)
        {
            EventSystem.current.SetSelectedGameObject(
                gameObject.GetComponent<TMP_InputField>().navigation.selectOnDown.gameObject
            );

            StartCoroutine(ActivateNavigationEventAfterTime(TIMER_ACTIVATE_NAV_EVENT));
        }
    }
    
    private IEnumerator ActivateNavigationEventAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        EventSystem.current.sendNavigationEvents = true;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnUserInterfaceSelectPerformed -= LobbyInputManager_OnSelectUIPressed;
        InputManager.Instance.OnUserInterfaceCancelPerformed -= LobbyInputManager_OnCancelUIPressed;
        InputManager.Instance.OnUserInterfaceUpPerformed -= LobbyInputManager_OnUpUIPressed;
        InputManager.Instance.OnUserInterfaceDownPerformed -= LobbyInputManager_OnDownUIPressed;
    }
}
