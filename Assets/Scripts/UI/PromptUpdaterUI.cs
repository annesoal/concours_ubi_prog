using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptUpdaterUI : MonoBehaviour
{
    [SerializeField] private InputManager.Binding associatedBinding;
    
    [SerializeField] private bool getAlternate;
    [SerializeField] private bool getBlackVersion;

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
        _promptImage.color = getBlackVersion ? Color.black : Color.white;
        
        if (getAlternate)
        {
            _promptImage.sprite = InputManager.Instance.GetBindingSpriteAlternate(associatedBinding);
        }
        else
        {
            _promptImage.sprite = InputManager.Instance.GetBindingSprite(associatedBinding);
        }
    }
}
