using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleLevelSelectUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform amuletLayout;
    [SerializeField] private SingleAmuletTemplateUI amuletTemplateUI;

    [SerializeField] private GameObject selectedOutline;

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

    public void OnSelect(BaseEventData eventData)
    {
        selectedOutline.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectedOutline.SetActive(false);
    }
}
