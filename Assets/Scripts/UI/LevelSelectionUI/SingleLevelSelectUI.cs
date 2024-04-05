using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using TMPro;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleLevelSelectUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform amuletLayout;
    [SerializeField] private SingleAmuletTemplateUI amuletTemplateUI;

    [SerializeField] private GameObject selectedOutline;

    public LevelSelectSO AssociatedLevelSO { get; private set; }

    /// <summary>
    /// Dans le but d'avoir une représentation UI cohérente des amulettes entre le client et le serveur.
    /// </summary>
    public void UpdateAmuletsToShowClientSide()
    {
        if (NetworkManager.Singleton.IsServer) { return; }

        int[] amuletsIdsShownByServer =
            AmuletLayoutSynchronizer.Instance.GetAmuletsIdsShownByServer(AssociatedLevelSO.levelScene);

        AmuletSaveLoad converter = new AmuletSaveLoad();

        List<AmuletSO> amuletsShownByServer =
            converter.AmuletsIDToAmulets(amuletsIdsShownByServer, AmuletLayoutSynchronizer.Instance.AmuletSelector.amulets);

        ShowAmuletsClientSide(amuletsShownByServer);
    }

    public void Show(LevelSelectSO levelSO)
    {
        AssociatedLevelSO = levelSO;

        nameText.text = AssociatedLevelSO.levelName;

        ShowAmuletsServerSide();
        
        BasicShowHide.Show(gameObject);
    }

    private void ShowAmuletsServerSide()
    {
        if (! NetworkManager.Singleton.IsServer) { return; }
        
        AmuletSaveLoad loader = new AmuletSaveLoad();
        
        List<AmuletSO> amuletsToShow =
            loader.GetAmuletsForScene(AssociatedLevelSO.levelScene, AmuletLayoutSynchronizer.Instance.AmuletSelector.amulets);

        AmuletLayoutSynchronizer.Instance.SaveServerSideAmuletsForLevel(AssociatedLevelSO.levelScene);

        foreach (AmuletSO toShow in amuletsToShow)
        {
            SingleAmuletTemplateUI templateInstance = Instantiate(amuletTemplateUI, amuletLayout);
            
            templateInstance.Show(toShow);
        }
    }
    
    private void ShowAmuletsClientSide(List<AmuletSO> amuletsToShow)
    {
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
