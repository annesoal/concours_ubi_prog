using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuUI : MonoBehaviour
{
    private void Awake()
    {
        showBuildingMenuButton.onClick.AddListener(() =>
        {
            InputManager.Instance.DisablePlayerInputMap();
            
            circularLayout.ShowLayout();
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
        
        circularLayout.HideLayout();
        
        UpdateLayoutVisuals();
    }

    private void UpdateLayoutVisuals()
    {
        foreach (BuildableObjectSO buildableObjectSo in SynchronizeBuilding.Instance.GetAllBuildableObjectSo().list)
        {
            circularLayout.AddObjectToLayout(buildableObjectSo);
        }
    }

    [SerializeField] private Button showBuildingMenuButton;
    [SerializeField] private CircularLayoutUI circularLayout;
    
    private void TowerDefenseManager_OnCurrentStateChanged(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue != TowerDefenseManager.State.TacticalPause)
        {
            BasicShowHide.Hide(showBuildingMenuButton.gameObject);
            BasicShowHide.Hide(circularLayout.gameObject);
            _playerIsNearWorkshop = false;
        }
    }
    
    private void Workshop_OnAnyWorkshopNearPlayer(object sender, EventArgs e)
    {
        BasicShowHide.Show(showBuildingMenuButton.gameObject);
        _playerIsNearWorkshop = true;
    }


    [SerializeField] private BuildingObjectOnGridUI buildingTowerOnGridUI;
    
    private void SingleTowerSelectUI_OnAnySingleBuildableObjectSelectUISelected
        (object sender, SingleBuildableObjectSelectUI.BuildableObjectData e)
    {
        BasicShowHide.Hide(circularLayout.gameObject);
        buildingTowerOnGridUI.Show(e.buildableObjectInfos);
    }

    private bool _playerIsNearWorkshop = false;
    
    private void InputManager_OnPlayerInteractPerformed(object sender, EventArgs e)
    {
        showBuildingMenuButton.onClick.Invoke();
    }
    
}
