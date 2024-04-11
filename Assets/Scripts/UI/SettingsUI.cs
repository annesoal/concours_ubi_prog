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
