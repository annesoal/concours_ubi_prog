using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelFocusUI : MonoBehaviour
{
    [SerializeField] private SingleLevelSelectUI levelDisplay;

    [SerializeField] private Button readyButton;
    [SerializeField] private Button cancelButton;

    private LevelSelectSO _selectedLevelSO;
    
    private void Awake()
    {
        readyButton.onClick.AddListener(OnReadyButtonClicked);
        
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    public void Show(LevelSelectSO toShow)
    {
        _selectedLevelSO = toShow;
        
        levelDisplay.Show(toShow);
        
        BasicShowHide.Show(gameObject);
    }

    private void OnReadyButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        
        // TODO SET SELECTED AMULET
        
        Loader.LoadNetwork(_selectedLevelSO.levelScene);
    }
    
    private void OnCancelButtonClicked()
    {
        BasicShowHide.Hide(gameObject);
        
        // TODO SHOW LevelSelectionUI again
        // event ?
    }

}
