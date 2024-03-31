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

    private void Awake()
    {
        int currentHorizontalLayout = 0;
        Transform currentVerticalLayout = Instantiate(levelVerticalLayout, transform);
        
        foreach (LevelSelectSO levelSO in selectableLevelsListSO.levels)
        {
            if (currentHorizontalLayout < maxHorizonalLayout)
            { 
                SingleLevelSelectUI levelSelectUI = Instantiate(singleLevelTemplateUI, currentVerticalLayout);
            }
            else
            {
                // Instantiate new vertical layout group
            }
            
            currentHorizontalLayout++;
        }
    }
}
