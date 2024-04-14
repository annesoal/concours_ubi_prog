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
    [Obsolete]
    public void UpdateAmuletsToShowClientSide()
    {
        if (NetworkManager.Singleton.IsServer) { return; }

        int[] amuletsIdsShownByServer =
            LevelSelectionSynchronizer.Instance.GetAmuletsIdsShownByServer(AssociatedLevelSO.levelScene);

        AmuletSaveLoad converter = new AmuletSaveLoad();

        //List<AmuletSO> amuletsShownByServer =
            //converter.AmuletsIDToAmulets(amuletsIdsShownByServer, LevelSelectionSynchronizer.Instance.AmuletSelector.amulets);

        //ShowAmuletsClientSide(amuletsShownByServer);
    }

    public void Show(LevelSelectSO levelSO)
    {
        EmptyAmuletsDisplay();
        
        AssociatedLevelSO = levelSO;

        nameText.text = AssociatedLevelSO.levelName;

        ShowAmuletsServerSide();
        
        BasicShowHide.Show(gameObject);
    }

    private void EmptyAmuletsDisplay()
    {
        foreach (Transform child in amuletLayout)
        {
            if (child.gameObject != amuletTemplateUI.gameObject)
            {
                Destroy(child.gameObject);
            }
        }
    }

    [Obsolete]
    private void ShowAmuletsServerSide()
    {
        if (! NetworkManager.Singleton.IsServer) { return; }
        
        AmuletSaveLoad loader = new AmuletSaveLoad();
        
        //List<AmuletSO> amuletsToShow =
            //loader.GetAmuletsForScene(AssociatedLevelSO.levelScene, LevelSelectionSynchronizer.Instance.AmuletSelector.amulets);
            List<AmuletSO> amuletsToShow = new List<AmuletSO>();

        LevelSelectionSynchronizer.Instance.SaveServerSideAmuletsForLevel(AssociatedLevelSO.levelScene);

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
