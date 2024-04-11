using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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
