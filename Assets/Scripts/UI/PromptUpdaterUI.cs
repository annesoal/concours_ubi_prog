using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptUpdaterUI : MonoBehaviour
{
    [SerializeField] private InputManager.Binding associatedBinding;

    private Image _promptImage;
    
    private void Start()
    {
        _promptImage = GetComponent<Image>();
        
        UpdateSprite();
        
        InputManager.Instance.OnInputRebindingCompleted += InputManager_OnInputRebindingCompleted;
    }

    private void InputManager_OnInputRebindingCompleted(object sender, EventArgs e)
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        _promptImage.sprite = InputManager.Instance.GetBindingSprite(associatedBinding);
    }
}
