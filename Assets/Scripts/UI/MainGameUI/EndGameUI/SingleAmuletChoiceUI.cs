using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleAmuletChoiceUI : MonoBehaviour
{
    [SerializeField] private Image amuletIcon;
    [SerializeField] private TextMeshProUGUI amuletDescriptionText;
    [SerializeField] private Button selectAmuletButton;

    public event EventHandler<OnAmuletSelectButtonClickedEventArgs> OnAmuletSelectButtonClicked;
    public class OnAmuletSelectButtonClickedEventArgs : EventArgs { public AdditionAmuletSO SelectedAmulet; }

    private AdditionAmuletSO _associatedAmuletSo;
    
    public void SetVisuals(AdditionAmuletSO amuletSo)
    {
        _associatedAmuletSo = amuletSo;

        amuletIcon.sprite = amuletSo.amuletIcon;
        amuletDescriptionText.text = amuletSo.description;
        
        selectAmuletButton.onClick.AddListener(() =>
        {
            OnAmuletSelectButtonClicked?.Invoke(this, new OnAmuletSelectButtonClickedEventArgs
            {
                SelectedAmulet = _associatedAmuletSo,
            });
        });
    }
    
    public void SetButtonAsSelected()
    {
        selectAmuletButton.Select();
    }
}
