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
    
    [Header("Tweening")]
    [SerializeField] private float showTweeningTime;
    [SerializeField] private float hideTweeningTime;
    
    private LTDescr _currentTween = null;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            if (_currentTween != null) { return; }
            
            toSelectAfterClosing.GetComponent<Selectable>().Select();
            Hide();
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
        if (_currentTween != null) { return; }
        
        transform.localScale = Vector3.zero;
        BasicShowHide.Show(gameObject);
        _currentTween = transform.LeanScale(Vector3.one, showTweeningTime).setEaseOutExpo().setOnComplete(() =>
        {
            _currentTween = null;
        });
        
        UpdateVisuals();
        EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
    }

    private void Hide()
    {
        _currentTween = transform.LeanScale(Vector3.zero, hideTweeningTime).setEaseOutExpo().setOnComplete(() =>
        {
            _currentTween = null;
            BasicShowHide.Hide(gameObject);
        });
    }

    private void CarryOutRebinding(InputManager.Binding toRebind)
    {
        ShowPressToRebindUI();
            
        InputManager.Instance.RebindBinding(toRebind, () =>
        {
            HidePressToRebindUI();
            UpdateVisuals();
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

    private void UpdateVisuals()
    { 
        UpdateMovementInputSprite();

        UpdateInteractionsInputSprite();

        UpdateBuildingInputSprite();
    }

    private void UpdateMovementInputSprite()
    {
        Sprite toSet = GetSpriteForBinding(InputManager.Binding.Up);
        if (toSet != null) { upImage.sprite = toSet; }
        
        toSet = GetSpriteForBinding(InputManager.Binding.Down);
        if (toSet != null) { downImage.sprite = toSet; }
        
        toSet = GetSpriteForBinding(InputManager.Binding.Left);
        if (toSet != null) { leftImage.sprite = toSet; }
        
        toSet = GetSpriteForBinding(InputManager.Binding.Right);
        if (toSet != null) { rightImage.sprite = toSet; }
    }
    
    private void UpdateInteractionsInputSprite()
    {
        Sprite toSet = GetSpriteForBinding(InputManager.Binding.Select);
        if (toSet != null) { selectImage.sprite = toSet; }
        
        toSet = GetSpriteForBinding(InputManager.Binding.Cancel);
        if (toSet != null) { cancelImage.sprite = toSet; }
        
        toSet = GetSpriteForBinding(InputManager.Binding.Confirm);
        if (toSet != null) { confirmImage.sprite = toSet; }
        
        toSet = GetSpriteForBinding(InputManager.Binding.Interact);
        if (toSet != null) { interactImage.sprite = toSet; }
    }
    
    private void UpdateBuildingInputSprite()
    {
        Sprite toSet = GetSpriteForBinding(InputManager.Binding.ShoulderLeft);
        if (toSet != null) { leftCarouselImage.sprite = toSet; }
        
        toSet = GetSpriteForBinding(InputManager.Binding.ShoulderRight);
        if (toSet != null) { rightCarouselImage.sprite = toSet; }
    }

    private Sprite GetSpriteForBinding(InputManager.Binding binding)
    {
        string overridePath = InputManager.Instance.GetBindingOverridePath(binding) ?? InputManager.Instance.GetBindingPath(binding);

        foreach (PairInputPathAndSpriteSO.PairInputPathAndSprite pair in pairInputPathAndSpriteSo.pairsList)
        {
            if (pair.path == overridePath)
            {
                return pair.sprite;
            }
        }

        return null;
    }
}
