using System;
using System.Collections;
using System.Collections.Generic;
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

    private LinkedList<SingleLevelSelectUI> _levelsSelectUI;
    private LinkedListNode<SingleLevelSelectUI> _selectedLevel;
    
    private void Awake()
    {
        int horizontalLayoutCount = 0;
        Transform currentVerticalLayout = Instantiate(levelVerticalLayout, transform);
        
        foreach (LevelSelectSO levelSO in selectableLevelsListSO.levels)
        {
            AddVerticalLayoutWhenFull(ref currentVerticalLayout, ref horizontalLayoutCount);
            
            InstantiateSingleLevelSelectTemplate(levelSO, currentVerticalLayout);
            
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
    }

    private void AddVerticalLayoutWhenFull(ref Transform currentVerticalLayout, ref int horizontalLayoutCount)
    {
            if (horizontalLayoutCount >= maxHorizonalLayout)
            { 
                currentVerticalLayout = Instantiate(levelVerticalLayout, transform);
                
                horizontalLayoutCount = 0;
            }
    }

    private void InstantiateSingleLevelSelectTemplate(LevelSelectSO levelSO, Transform currentVerticalLayout)
    {
        SingleLevelSelectUI templateInstance = Instantiate(singleLevelTemplateUI, currentVerticalLayout);
                
        templateInstance.gameObject.SetActive(true);
                
        templateInstance.Show(levelSO);
            
        _levelsSelectUI.AddLast(templateInstance);
    }
    
    private void InputManager_OnLeftUI(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            UpdateSelectedLevel(_selectedLevel.Next ?? _levelsSelectUI.First);
        }
    }
    
    private void InputManager_OnRightUI(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            UpdateSelectedLevel(_selectedLevel.Previous ?? _levelsSelectUI.Last);
        }
    }
    
    private void InputManager_OnUpUI(object sender, EventArgs e)
    {
        if (gameObject.activeSelf) { return; }
        
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
        if (! gameObject.activeSelf) { return; }

        LinkedListNode<SingleLevelSelectUI> newSelectedLevel = null;
        
        for (int i = 0; i < maxHorizonalLayout; i++)
        {
            newSelectedLevel = _selectedLevel.Next;

            if (newSelectedLevel == null) { return; }
        }

        UpdateSelectedLevel(newSelectedLevel);
    }

    private void UpdateSelectedLevel(LinkedListNode<SingleLevelSelectUI> newSelectedLevel)
    {
        Debug.Assert(_selectedLevel != null, nameof(_selectedLevel) + " != null");
        
        _selectedLevel = newSelectedLevel;
        EventSystem.current.SetSelectedGameObject(_selectedLevel.Value.gameObject);
    }
    
    private void InputManager_OnSelectUI(object sender, EventArgs e)
    {
        // TODO show level focus ui for this level !
    }
}
