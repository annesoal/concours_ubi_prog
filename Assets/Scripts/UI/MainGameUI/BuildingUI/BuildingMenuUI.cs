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
            
                circularLayout.ShowLayout();
            }
        });
    }

    private void Start()
    {
        BasicShowHide.Hide(showBuildingMenuButton.gameObject);
        
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        
        Workshop.OnAnyWorkshopNearPlayer += Workshop_OnAnyWorkshopNearPlayer;
        
        SingleBuildableObjectSelectUI.OnAnySingleBuildableObjectSelectUISelected +=
            SingleTowerSelectUI_OnAnySingleBuildableObjectSelectUISelected;
        
        InputManager.Instance.OnPlayerInteractPerformed += InputManager_OnPlayerInteractPerformed;
        
        UpdateLayoutVisuals();
    }

    private void UpdateLayoutVisuals()
    {
        foreach (BuildableObjectSO buildableObjectSo in SynchronizeBuilding.Instance.GetAllBuildableObjectSo().list)
        {
            circularLayout.AddObjectToLayout(buildableObjectSo);
        }
    }

    private bool IsBuildingInactive()
    {
        return !circularLayout.gameObject.activeSelf && !buildingTowerOnGridUI.gameObject.activeSelf;
    }

    [SerializeField] private Button showBuildingMenuButton;
    [SerializeField] private CircularLayoutUI circularLayout;
    
    private void TowerDefenseManager_OnCurrentStateChanged(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue != TowerDefenseManager.State.TacticalPause)
        {
            // Aucune sélection de UI lors de la sortie de la pause tactique
            EventSystem.current.SetSelectedGameObject(null);
            
            BasicShowHide.Hide(showBuildingMenuButton.gameObject);
            BasicShowHide.Hide(circularLayout.gameObject);
            BasicShowHide.Hide(buildingTowerOnGridUI.gameObject);
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
    
    private void SingleTowerSelectUI_OnAnySingleBuildableObjectSelectUISelected
        (object sender, SingleBuildableObjectSelectUI.BuildableObjectData e)
    {
        BasicShowHide.Hide(circularLayout.gameObject);
        
        if (e.buildableObjectInfos.type == BuildableObjectSO.TypeOfBuildableObject.Tower)
        {
            buildingTowerOnGridUI.Show(e.buildableObjectInfos);
        }
        else
        {
            buildingTrapOnGridUI.Show(e.buildableObjectInfos);
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
