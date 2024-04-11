using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Rebinding")]
    [SerializeField] private GameObject pressToRebindUI;
    
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    
    [SerializeField] private Button selectButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button interactButton;
    
    [Header("Close")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Selectable toSelectAfterClosing;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            toSelectAfterClosing.GetComponent<Selectable>().Select();
            BasicShowHide.Hide(gameObject);
        });
        
        upButton.onClick.AddListener(() =>
        {
            CarryOutRebinding(InputManager.Binding.Up);
        });
        
        downButton.onClick.AddListener(() =>
        {
            CarryOutRebinding(InputManager.Binding.Down);
        });
        
        leftButton.onClick.AddListener(() =>
        {
            CarryOutRebinding(InputManager.Binding.Left);
        });
        
        rightButton.onClick.AddListener(() =>
        {
            CarryOutRebinding(InputManager.Binding.Right);
        });
        
        selectButton.onClick.AddListener(() =>
        {
            CarryOutRebinding(InputManager.Binding.Select);
        });
        
        cancelButton.onClick.AddListener(() =>
        {
            CarryOutRebinding(InputManager.Binding.Cancel);
        });
        
        confirmButton.onClick.AddListener(() =>
        {
            CarryOutRebinding(InputManager.Binding.Confirm);
        });
        
        interactButton.onClick.AddListener(() =>
        {
            CarryOutRebinding(InputManager.Binding.Interact);
        });
        
        BasicShowHide.Hide(gameObject);
    }
    
    public void Show()
    {
        BasicShowHide.Show(gameObject);
        EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
    }

    private void CarryOutRebinding(InputManager.Binding toRebind)
    {
        ShowPressToRebindUI();
            
        InputManager.Instance.RebindBinding(toRebind, () =>
        {
            HidePressToRebindUI();
            // TODO UpdateVisuals
        });
    }

    private void ShowPressToRebindUI()
    {
        BasicShowHide.Show(pressToRebindUI);
    }
    
    private void HidePressToRebindUI()
    {
        
        BasicShowHide.Hide(pressToRebindUI);
    }
}
