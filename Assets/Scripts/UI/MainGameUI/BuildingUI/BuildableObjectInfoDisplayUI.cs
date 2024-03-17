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
        SingleBuildableObjectSelectUI.OnAnySingleBuildableObjectSelectUISelected +=
            SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUISelected;
    }

    private GameObject preview;
    
    private void SingleBuildableObjectSelectUI_OnAnySingleBuildableObjectSelectUIHoveredEnter
        (object sender, SingleBuildableObjectSelectUI.BuildableObjectData e)
    {
        BasicShowHide.Show(gameObject);
        
        descriptionText.text = e.buildableObjectInfos.description;
        
        preview = Instantiate(e.buildableObjectInfos.visuals, buildableObjectVisualsPosition);
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

}
