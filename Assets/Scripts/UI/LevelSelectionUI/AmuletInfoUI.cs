using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using TMPro;
using UnityEngine;

public class AmuletInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;

    private AmuletSO _currentShowcaseAmulet;

    private void Start()
    {
        AmuletSelectionButton.OnAmuletButtonSelected += AmuletSelectionButton_OnAmuletButtonSelected;
        AmuletSelectionButton.OnAmuletButtonDeselected += AmuletSelectionButton_OnAmuletButtonDeselected;
    }

    private void AmuletSelectionButton_OnAmuletButtonSelected
        (object sender, AmuletSelectionButton.OnAmuletButtonSelectedEventArgs e)
    {
        descriptionText.color = Color.white;

        _currentShowcaseAmulet = e.AmuletSo;
    }

    private void AmuletSelectionButton_OnAmuletButtonDeselected
        (object sender, AmuletSelectionButton.OnAmuletButtonDeselectedEventArgs e)
    {
        if (_currentShowcaseAmulet == e.AmuletSo)
        {
            descriptionText.color = Color.yellow;
            descriptionText.text = "Select An Amulet To See Its Description";

            _currentShowcaseAmulet = e.AmuletSo;
        }
    }
}
