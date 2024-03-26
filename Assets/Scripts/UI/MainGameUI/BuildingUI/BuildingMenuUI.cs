using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingMenuUI : MonoBehaviour
{
    private void Awake()
    {
        showBuildingMenuButton.onClick.AddListener(() =>
        {
            if (IsBuildingInactive())
            {
                InputManager.Instance.DisablePlayerInputMap();
            
                buildingCarrouselUI.Show();
            }
        });
    }

    private void Start()
    {
        BasicShowHide.Hide(showBuildingMenuButton.gameObject);
        
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        
        Workshop.OnAnyWorkshopNearPlayer += Workshop_OnAnyWorkshopNearPlayer;
        
        buildingCarrouselUI.OnBuildingSelected += BuildingCarrouselUI_OnBuildingSelected;
        
        InputManager.Instance.OnPlayerInteractPerformed += InputManager_OnPlayerInteractPerformed;
    }

    private bool IsBuildingInactive()
    {
        return !buildingCarrouselUI.gameObject.activeSelf &&
               !buildingTowerOnGridUI.gameObject.activeSelf &&
               !buildingTrapOnGridUI.gameObject.activeSelf;
    }

    [SerializeField] private Button showBuildingMenuButton;
    [SerializeField] private BuildingCarrouselUI buildingCarrouselUI;
    
    private void TowerDefenseManager_OnCurrentStateChanged(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue != TowerDefenseManager.State.TacticalPause)
        {
            // Aucune sélection de UI lors de la sortie de la pause tactique
            EventSystem.current.SetSelectedGameObject(null);
            
            BasicShowHide.Hide(showBuildingMenuButton.gameObject);
            buildingCarrouselUI.Hide();
            buildingTowerOnGridUI.Hide();
            BasicShowHide.Hide(buildingTrapOnGridUI.gameObject);
            
            _playerIsNearWorkshop = false;
        }
    }
    
    private void Workshop_OnAnyWorkshopNearPlayer(object sender, EventArgs e)
    {
        BasicShowHide.Show(showBuildingMenuButton.gameObject);
        _playerIsNearWorkshop = true;
    }


    [SerializeField] private BuildingTowerOnGridUI buildingTowerOnGridUI;
    [SerializeField] private BuildingTrapOnGridUI buildingTrapOnGridUI;

    private void BuildingCarrouselUI_OnBuildingSelected(object sender, BuildingCarrouselUI.OnBuildingSelectedEventArgs e)
    {
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

    private bool _playerIsNearWorkshop = false;
    
    private void InputManager_OnPlayerInteractPerformed(object sender, EventArgs e)
    {
        if (_playerIsNearWorkshop)
        {
            showBuildingMenuButton.onClick.Invoke();
        }
    }
    
}
