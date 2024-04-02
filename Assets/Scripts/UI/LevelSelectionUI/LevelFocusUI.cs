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
    private AmuletSO _selectedAmulet;
    
    private void Awake()
    {
        readyButton.onClick.AddListener(OnReadyButtonClicked);
        
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
        
        BasicShowHide.Hide(gameObject);
    }

    private void Start()
    {
        SingleAmuletTemplateUI.OnAnySingleAmuletChose += SingleAmuletTemplateUI_OnAnySingleAmuletChose;
    }

    public void Show(LevelSelectSO toShow)
    {
        _selectedLevelSO = toShow;
        _selectedAmulet = null;
        
        levelDisplay.Show(toShow);
        
        BasicShowHide.Show(gameObject);
        
        StartCoroutine(EnableNavEventsTimer());
    }

    private const float TIMER_ACTIVATE_NAV_EVENT = 0.08f;
    private IEnumerator EnableNavEventsTimer()
    {
        yield return new WaitForSeconds(TIMER_ACTIVATE_NAV_EVENT);
        EventSystem.current.sendNavigationEvents = true;
        readyButton.Select();
    }

    private void OnReadyButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        
        if (_selectedAmulet != null)
        {
            AmuletSelector.PlayerAmuletSelection = _selectedAmulet.ID;
        }
        
        Loader.LoadNetwork(_selectedLevelSO.levelScene);
    }

    [Header("Level Selection UI")]
    [SerializeField] private LevelSelectionUI levelSelectionUI;
    private void OnCancelButtonClicked()
    {
        BasicShowHide.Hide(gameObject);

        levelSelectionUI.Show();
    }
    
    private void SingleAmuletTemplateUI_OnAnySingleAmuletChose
        (object sender, SingleAmuletTemplateUI.OnAnySingleAmuletChoseEventArgs e)
    {
        if (_selectedAmulet == e.AmuletSo)
        {
            // Selected twice
            _selectedAmulet = null;
        }
        else
        {
            _selectedAmulet = e.AmuletSo;
        }
    }

}
