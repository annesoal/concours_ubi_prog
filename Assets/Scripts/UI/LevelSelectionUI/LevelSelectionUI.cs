using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void Start()
    {
        // TODO CONNECT TO INPUT
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
}
