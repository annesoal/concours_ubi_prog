using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = System.Diagnostics.Debug;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private LevelSelectListSO selectableLevelsListSO;

    [SerializeField] private SingleLevelSelectUI singleLevelTemplateUI;
    
    [Header("Layouts")]
    [SerializeField] private int maxHorizonalLayout;
    [SerializeField] private Transform levelVerticalLayout;
    [SerializeField] private Transform levelHorizontalLayout;

    private LinkedList<SingleLevelSelectUI> _levelsSelectUI;
    private LinkedListNode<SingleLevelSelectUI> _selectedLevel;
    
    private void Awake()
    {
        _levelsSelectUI = new LinkedList<SingleLevelSelectUI>();
        
        int horizontalLayoutCount = 0;
        Transform currentHorizontalLayout = Instantiate(levelHorizontalLayout, levelVerticalLayout);
        currentHorizontalLayout.gameObject.SetActive(true);
        
        foreach (LevelSelectSO levelSO in selectableLevelsListSO.levels)
        {
            AddHorizontalLayoutWhenFull(ref currentHorizontalLayout, ref horizontalLayoutCount);
            
            InstantiateSingleLevelSelectTemplate(levelSO, currentHorizontalLayout);
            
            horizontalLayoutCount++;
        }

        _selectedLevel = _levelsSelectUI.First;
        EventSystem.current.SetSelectedGameObject(_selectedLevel.Value.gameObject);
    }

    private void Start()
    {
        LevelSelectionInputManager.Instance.OnLeftUI += InputManager_OnLeftUI;
        LevelSelectionInputManager.Instance.OnRightUI += InputManager_OnRightUI;
        LevelSelectionInputManager.Instance.OnUpUI += InputManager_OnUpUI;
        LevelSelectionInputManager.Instance.OnDownUI += InputManager_OnDownUI;
        
        LevelSelectionInputManager.Instance.OnSelectUI += InputManager_OnSelectUI;

        EventSystem.current.sendNavigationEvents = false;
    }
    
    public void Show()
    {
        EventSystem.current.sendNavigationEvents = false;
        
        BasicShowHide.Show(gameObject);
        
        EventSystem.current.SetSelectedGameObject(_selectedLevel.Value.gameObject);
    }

    private void AddHorizontalLayoutWhenFull(ref Transform currentHorizontalLayout, ref int horizontalLayoutCount)
    {
            if (horizontalLayoutCount >= maxHorizonalLayout)
            { 
                currentHorizontalLayout = Instantiate(levelHorizontalLayout, levelVerticalLayout);
                
                currentHorizontalLayout.gameObject.SetActive(true);
                
                horizontalLayoutCount = 0;
            }
    }

    private void InstantiateSingleLevelSelectTemplate(LevelSelectSO levelSO, Transform currentHorizontalLayout)
    {
        SingleLevelSelectUI templateInstance = Instantiate(singleLevelTemplateUI, currentHorizontalLayout);
                
        templateInstance.gameObject.SetActive(true);
                
        templateInstance.Show(levelSO);
            
        _levelsSelectUI.AddLast(templateInstance);
    }
    
    private void InputManager_OnLeftUI(object sender, EventArgs e)
    {
        if (CanHandleInput())
        {
            UpdateSelectedLevel(_selectedLevel.Next ?? _levelsSelectUI.First);
        }
    }
    
    private void InputManager_OnRightUI(object sender, EventArgs e)
    {
        if (CanHandleInput())
        {
            UpdateSelectedLevel(_selectedLevel.Previous ?? _levelsSelectUI.Last);
        }
    }
    
    private void InputManager_OnUpUI(object sender, EventArgs e)
    {
        if (! CanHandleInput()) { return; }
        
        LinkedListNode<SingleLevelSelectUI> newSelectedLevel = null;
        
        for (int i = 0; i < maxHorizonalLayout; i++)
        {
            newSelectedLevel = _selectedLevel.Previous;

            if (newSelectedLevel == null) { return; }
        }

        UpdateSelectedLevel(newSelectedLevel);
    }
    private void InputManager_OnDownUI(object sender, EventArgs e)
    {
        if (! CanHandleInput()) { return; }

        LinkedListNode<SingleLevelSelectUI> newSelectedLevel = null;
        
        for (int i = 0; i < maxHorizonalLayout; i++)
        {
            newSelectedLevel = _selectedLevel.Next;

            if (newSelectedLevel == null) { return; }
        }

        UpdateSelectedLevel(newSelectedLevel);
    }
    
    [Header("Level Focus UI")]
    [SerializeField] private LevelFocusUI levelFocusUI;
    
    private void InputManager_OnSelectUI(object sender, EventArgs e)
    {
        if (CanHandleInput())
        {
            BasicShowHide.Hide(gameObject);
        
            levelFocusUI.Show(_selectedLevel.Value.AssociatedLevelSO);
        }
    }

    private bool CanHandleInput()
    {
        return gameObject.activeSelf && NetworkManager.Singleton.IsServer;
    }

    private void UpdateSelectedLevel(LinkedListNode<SingleLevelSelectUI> newSelectedLevel)
    {
        Debug.Assert(_selectedLevel != null, nameof(_selectedLevel) + " != null");
        
        _selectedLevel = newSelectedLevel;
        EventSystem.current.SetSelectedGameObject(_selectedLevel.Value.gameObject);
    }
}
