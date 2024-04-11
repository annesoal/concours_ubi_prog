using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private SettingsUI settingsUI;

    private void Awake()
    {
        settingsButton.onClick.AddListener(() =>
        {
            settingsUI.Show();
        });
    }
}
