using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class BuildableObjectInfoDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Transform buildableObjectVisualsPosition;
    
    private void Start()
    {
        SingleBuildableObjectSelectUI.OnAnySingleBuildableObjectSelectUIHoveredEnter +=
            SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUIHoveredEnter;
        SingleBuildableObjectSelectUI.OnAnySingleBuildableObjectSelectUIHoveredExit +=
            SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUIHoveredExit;
    }

    private void SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUIHoveredEnter
        (object sender, SingleBuildableObjectSelectUI.BuildableObjectData e)
    {
        BasicShowHide.Show(gameObject);
        
        descriptionText.text = e.buildableObjectInfos.description;
        
        GameObject preview = Instantiate(e.buildableObjectInfos.visuals, buildableObjectVisualsPosition);
        preview.GetComponent<BuildableObjectVisuals>().ShowPreview();
    }
    
    private void SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUIHoveredExit
        (object sender, SingleBuildableObjectSelectUI.BuildableObjectData e)
    {
        BasicShowHide.Hide(gameObject);
    }

}
