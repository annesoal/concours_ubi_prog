using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject pressToRebindUI;
    [SerializeField] private PairInputPathAndSpriteSO pairInputPathAndSpriteSo;
    
    [Header("Rebinding Movement")]
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    
    [Header("Movement sprite")]
    [SerializeField] private Image upImage;
    [SerializeField] private Image downImage;
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;
    
    [Header("Rebinding Interactions")]
    [SerializeField] private Button selectButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button interactButton;
    
    [Header("Interactions sprite")]
    [SerializeField] private Image selectImage;
    [SerializeField] private Image cancelImage;
    [SerializeField] private Image confirmImage;
    [SerializeField] private Image interactImage;
    
    [Header("Rebinding Building")]
    [SerializeField] private Button leftCarouselButton;
    [SerializeField] private Button rightCarouselButton;
    
    [Header("Building sprite")]
    [SerializeField] private Image leftCarouselImage;
    [SerializeField] private Image rightCarouselImage;
    
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
        
        leftCarouselButton.onClick.AddListener(() =>
        {
            CarryOutRebinding(InputManager.Binding.ShoulderLeft);
        });
        
        rightCarouselButton.onClick.AddListener(() =>
        {
            CarryOutRebinding(InputManager.Binding.ShoulderRight);
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
