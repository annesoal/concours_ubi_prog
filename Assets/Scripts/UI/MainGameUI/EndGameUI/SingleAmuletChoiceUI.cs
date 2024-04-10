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
    public class OnAmuletSelectButtonClickedEventArgs : EventArgs { public AmuletSO SelectedAmulet; }

    private AmuletSO _associatedAmuletSo;
    
    public void SetVisuals(AmuletSO amuletSo)
    {
        amuletIcon.sprite = amuletSo.amuletIcon;
        amuletDescriptionText.text = amuletSo.description;
        _associatedAmuletSo = amuletSo;
        
        selectAmuletButton.onClick.AddListener(() =>
        {
            OnAmuletSelectButtonClicked?.Invoke(this, new OnAmuletSelectButtonClickedEventArgs
            {
                SelectedAmulet = _associatedAmuletSo,
            });
        });
    }
}
