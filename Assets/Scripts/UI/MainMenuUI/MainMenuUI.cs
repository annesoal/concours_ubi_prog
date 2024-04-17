using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Play")]
    [SerializeField] private Button playButton;
    
    [Header("Settings")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private SettingsUI settingsUI;
    
    [Header("Quit")]
    [SerializeField] private Button quitButton;
    
    [Header("Credits")]
    [SerializeField] private CreditsUI creditsUI;
    [SerializeField] private Button creditButton;
    
    [Header("Tutorial")]
    [SerializeField] private Button tutorialButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.LobbyScene);
        });
        
        settingsButton.onClick.AddListener(() =>
        {
            settingsUI.Show();
        });
        
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        
        creditButton.onClick.AddListener(() =>
        {
            creditsUI.ShowCredits();
        });
    }
}
