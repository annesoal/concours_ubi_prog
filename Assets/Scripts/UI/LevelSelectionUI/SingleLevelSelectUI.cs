using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class SingleLevelSelectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform amuletLayout;
    [SerializeField] private SingleAmuletTemplateUI amuletTemplateUI;
    
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private LevelSelectSO _associatedLevelSO;
    public void Show(LevelSelectSO levelSO)
    {
        _associatedLevelSO = levelSO;

        nameText.text = _associatedLevelSO.levelName;
        
        ShowAmulets();
        
        BasicShowHide.Show(gameObject);
    }

    private void ShowAmulets()
    {
        AmuletSaveLoad loader = new AmuletSaveLoad();
        
        List<AmuletSO> amuletsToShow = loader.GetAmuletsForScene(_associatedLevelSO.levelScene);

        foreach (AmuletSO toShow in amuletsToShow)
        {
            SingleAmuletTemplateUI templateInstance = Instantiate(amuletTemplateUI, amuletLayout);
            
            templateInstance.Show(toShow);
        }
    }
}
