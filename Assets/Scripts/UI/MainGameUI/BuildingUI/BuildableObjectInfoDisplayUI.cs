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
            SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUIHoveredEnter;
        SingleBuildableObjectSelectUI.OnAnyDeselectUI +=
            SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUIHoveredExit;
        SingleBuildableObjectSelectUI.OnAnySingleBuildableObjectSelectUISelected +=
            SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUISelected;

        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
    }

    private GameObject preview;
    private const float PREVIEW_DISTANCE = 10f;
    
    private void SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUIHoveredEnter
        (object sender, SingleBuildableObjectSelectUI.BuildableObjectData e)
    {
        BasicShowHide.Show(gameObject);
        
        descriptionText.text = e.buildableObjectInfos.description;
        
        preview = Instantiate(e.buildableObjectInfos.visuals);

        GameObject toFollow = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject;
        preview.AddComponent<FollowTransform>().SetFollowParameters(toFollow, PREVIEW_DISTANCE, FollowTransform.DirectionOfFollow.Front);
            
        preview.GetComponent<BuildableObjectVisuals>().ShowPreview();
    }
    
    private void SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUIHoveredExit
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
        
        Destroy(preview);
        preview = null;
    }
    
    private void TowerDefenseManager_OnCurrentStateChanged
        (object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        ClearDisplay();
    }


}
