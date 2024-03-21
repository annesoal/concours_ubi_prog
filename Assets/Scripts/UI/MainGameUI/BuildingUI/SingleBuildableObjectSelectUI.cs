using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private void Start()
    {
        SingleBuildableObjectSelectButtonUI selectButtonUI =
            selectionButton.GetComponent<SingleBuildableObjectSelectButtonUI>();
        
        selectButtonUI.OnButtonSelectedByController += SelectButtonUI_OnButtonSelectedByController;
        selectButtonUI.OnButtonDeselectedByController += SelectButtonUI_OnButtonDeselectedByController;
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

    public static event EventHandler<BuildableObjectData> OnAnySelectUI;
    public static event EventHandler<BuildableObjectData> OnAnyDeselectUI;

    public class BuildableObjectData : EventArgs
    {
        public BuildableObjectSO buildableObjectInfos;
    }
    public void OnEventTrigger_PointerEnter()
    {
        OnAnySelectUI?.Invoke(this, new BuildableObjectData
        {
            buildableObjectInfos = correspondingBuildableObjectSo
        });
    }

    public void OnEventTrigger_PointerExit()
    {
        OnAnyDeselectUI?.Invoke(this, new BuildableObjectData
        {
            buildableObjectInfos = correspondingBuildableObjectSo
        });
    }

    public void SelectThisSelectionButton()
    {
        EventSystem.current.SetSelectedGameObject(selectionButton.gameObject);
        EmitSelectionSignal(OnAnySelectUI);
    }
    
    private void SelectButtonUI_OnButtonSelectedByController(object sender, EventArgs e)
    {
        EmitSelectionSignal(OnAnySelectUI);
    }
    
    private void SelectButtonUI_OnButtonDeselectedByController(object sender, EventArgs e)
    {
        EmitSelectionSignal(OnAnyDeselectUI);
    }

    private void EmitSelectionSignal(EventHandler<BuildableObjectData> toEmit)
    {
        toEmit?.Invoke(this, new BuildableObjectData
        {
            buildableObjectInfos = correspondingBuildableObjectSo
        });
    }
    
    public static void ResetStaticData()
    {
        OnAnySelectUI = null;
        OnAnyDeselectUI = null;
        OnAnySingleBuildableObjectSelectUISelected = null;
    }
}
