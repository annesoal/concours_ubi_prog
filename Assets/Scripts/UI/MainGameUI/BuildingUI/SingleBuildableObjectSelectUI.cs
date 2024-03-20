using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleBuildableObjectSelectUI : MonoBehaviour
{
    [SerializeField] private Button selectionButton;

    private BuildableObjectSO correspondingBuildableObjectSo;

    public static event EventHandler<BuildableObjectData> OnAnySingleBuildableObjectSelectUISelected;
    
    private void Awake()
    {
        selectionButton.onClick.AddListener(() =>
        {
            OnAnySingleBuildableObjectSelectUISelected?.Invoke(this, new BuildableObjectData
            {
                buildableObjectInfos = correspondingBuildableObjectSo
            });
        });
    }

    public void SetCorrespondingTowerSO(BuildableObjectSO toSet)
    {
        correspondingBuildableObjectSo = toSet;
        
        SetIcon(toSet.icon);
    }
    
    public void SetIcon(Sprite icon)
    {
        selectionButton.image.sprite = icon;
    }

    public static event EventHandler<BuildableObjectData> OnAnySingleBuildableObjectSelectUIHoveredEnter;
    public static event EventHandler<BuildableObjectData> OnAnySingleBuildableObjectSelectUIHoveredExit;

    public class BuildableObjectData : EventArgs
    {
        public BuildableObjectSO buildableObjectInfos;
    }
    public void OnEventTrigger_PointerEnter()
    {
        Debug.Log("MOUSE OVER !");
        OnAnySingleBuildableObjectSelectUIHoveredEnter?.Invoke(this, new BuildableObjectData
        {
            buildableObjectInfos = correspondingBuildableObjectSo
        });
    }

    public void OnEventTrigger_PointerExit()
    {
        Debug.Log("Mouse exit");
        OnAnySingleBuildableObjectSelectUIHoveredExit?.Invoke(this, new BuildableObjectData
        {
            buildableObjectInfos = correspondingBuildableObjectSo
        });
    }

    public void SelectThisSelectionButton()
    {
        selectionButton.Select();
    }
    
    public static void ResetStaticData()
    {
        OnAnySingleBuildableObjectSelectUIHoveredEnter = null;
        OnAnySingleBuildableObjectSelectUIHoveredExit = null;
        OnAnySingleBuildableObjectSelectUISelected = null;
    }
}
