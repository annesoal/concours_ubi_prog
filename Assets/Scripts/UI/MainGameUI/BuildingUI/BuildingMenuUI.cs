using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UI;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingMenuUI : MonoBehaviour
{
    private void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        
        Workshop.OnAnyWorkshopNearPlayer += Workshop_OnAnyWorkshopNearPlayer;
        
        buildingCarrouselUI.OnBuildingSelected += BuildingCarrouselUI_OnBuildingSelected;
        
        buildingTrapOnGridUI.OnCloseUI += BuildingTrapOnGridUI_OnCloseUI;
        buildingTowerOnGridUI.OnCloseUI += BuildingTowerOnGridUI_OnCloseUI;
    }

    private bool IsBuildingInactive()
    {
        return !buildingCarrouselUI.gameObject.activeSelf &&
               !buildingTowerOnGridUI.gameObject.activeSelf &&
               !buildingTrapOnGridUI.gameObject.activeSelf;
    }

    [SerializeField] private BuildingCarrouselUI buildingCarrouselUI;
    
    private void TowerDefenseManager_OnCurrentStateChanged(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue != TowerDefenseManager.State.TacticalPause)
        {
            InputManager.Instance.DisableUserInterfaceInputMap();
            
            // Aucune sélection de UI lors de la sortie de la pause tactique
            EventSystem.current.SetSelectedGameObject(null);
            
            buildingCarrouselUI.Hide();
            buildingTowerOnGridUI.Hide();
            buildingTrapOnGridUI.Hide();
        }
    }
    
    private void Workshop_OnAnyWorkshopNearPlayer(object sender, EventArgs e)
    {
        if (IsBuildingInactive())
        {
            ShowBuildingCarrouselUI();
        }
    }

    private void ShowBuildingCarrouselUI()
    {
        InputManager.Instance.EnableUserInterfaceInputMap();
            
        buildingCarrouselUI.Show();
    }


    [SerializeField] private BuildingTowerOnGridUI buildingTowerOnGridUI;
    [SerializeField] private BuildingTrapOnGridUI buildingTrapOnGridUI;

    private void BuildingCarrouselUI_OnBuildingSelected(object sender, BuildingCarrouselUI.OnBuildingSelectedEventArgs e)
    {
        InputManager.Instance.DisablePlayerInputMap();
        
        buildingCarrouselUI.HideForNextBuildStep();
        
        if (e.SelectedBuildableObjectSO.type == BuildableObjectSO.TypeOfBuildableObject.Tower)
        {
            buildingTowerOnGridUI.Show(e.SelectedBuildableObjectSO);
        }
        else
        {
            buildingTrapOnGridUI.Show(e.SelectedBuildableObjectSO);
        }
    }

    private void BuildingTrapOnGridUI_OnCloseUI(object sender, EventArgs e)
    {
        ShowBuildingCarrouselUI();
    }
    
    private void BuildingTowerOnGridUI_OnCloseUI(object sender, EventArgs e)
    {
        ShowBuildingCarrouselUI();
    }
}
