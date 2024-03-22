using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UI;
using UnityEngine;

public class BuildableObjectInfoDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    private void Start()
    {
        SingleBuildableObjectSelectUI.OnAnySelectUI +=
            SingleBuildableObjectSelectUI_OnAnySelectUI;
        SingleBuildableObjectSelectUI.OnAnyDeselectUI +=
            SingleBuildableObjectSelectUI_OnAnyDeselectUI;
        SingleBuildableObjectSelectUI.OnAnySingleBuildableObjectSelectUISelected +=
            SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUISelected;

        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        
        BasicShowHide.Hide(gameObject);
    }

    private GameObject _preview;
    private const float PREVIEW_DISTANCE = 10f;
    
    private void SingleBuildableObjectSelectUI_OnAnySelectUI
        (object sender, SingleBuildableObjectSelectUI.BuildableObjectData e)
    {
        ClearOldPreview();
        
        ClearOldMaterialCostUI();
        
        BasicShowHide.Show(gameObject);
        
        descriptionText.text = e.buildableObjectInfos.description;
        
        ShowPreviewInFrontOfCamera(e);
        
        ShowMaterialCost(e);
    }

    private void ShowPreviewInFrontOfCamera(SingleBuildableObjectSelectUI.BuildableObjectData buildableObjectData)
    {
        _preview = Instantiate(buildableObjectData.buildableObjectInfos.visuals);

        GameObject toFollow = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject;
        _preview.AddComponent<FollowTransform>().SetFollowParameters(toFollow, PREVIEW_DISTANCE, FollowTransform.DirectionOfFollow.Front);
            
        _preview.GetComponent<BuildableObjectVisuals>().ShowPreview();
    }

    private void ShowMaterialCost(SingleBuildableObjectSelectUI.BuildableObjectData buildableObjectData)
    {
        foreach (BuildableObjectSO.BuildingMaterialAndQuantityPair pair in buildableObjectData.buildableObjectInfos.materialAndQuantityPairs)
        {
            CentralizedInventory.Instance.ShowCostForResource(pair.buildingMaterialSO, pair.quantityOfMaterialRequired);
        }
    }

    private void ClearOldPreview()
    {
        if (_preview != null)
        {
            Destroy(_preview);
        }
    }
    
    private void ClearOldMaterialCostUI()
    {
        CentralizedInventory.Instance.ClearAllMaterialsCostUI();
    }
    
    private void SingleBuildableObjectSelectUI_OnAnyDeselectUI
        (object sender, SingleBuildableObjectSelectUI.BuildableObjectData e)
    {
        ClearDisplay();
    }

    private void SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUISelected
        (object sender, SingleBuildableObjectSelectUI.BuildableObjectData e)
    {
        ClearDisplay();
    }

    private void ClearDisplay()
    {
        BasicShowHide.Hide(gameObject);
        
        Destroy(_preview);
        _preview = null;
    }
    
    private void TowerDefenseManager_OnCurrentStateChanged
        (object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        ClearDisplay();
    }


}
