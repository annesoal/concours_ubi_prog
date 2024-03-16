using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleTowerSelectUI : MonoBehaviour
{
    [SerializeField] private Button selectionButton;

    private BuildableObjectSO correspondingTowerSO;

    public static event EventHandler<TowerData> OnAnySingleTowerSelectUISelected;
    
    private void Awake()
    {
        selectionButton.onClick.AddListener(() =>
        {
            OnAnySingleTowerSelectUISelected?.Invoke(this, new TowerData
            {
                towerInfos = correspondingTowerSO
            });
        });
    }

    public void SetCorrespondingTowerSO(BuildableObjectSO toSet)
    {
        correspondingTowerSO = toSet;
        
        SetIcon(toSet.icon);
    }
    
    public void SetIcon(Sprite icon)
    {
        selectionButton.image.sprite = icon;
    }

    public static event EventHandler<TowerData> OnAnySingleTowerSelectUIHoveredEnter;
    public static event EventHandler<TowerData> OnAnySingleTowerSelectUIHoveredExit;

    public class TowerData : EventArgs
    {
        public BuildableObjectSO towerInfos;
    }
    public void OnEventTrigger_PointerEnter()
    {
        Debug.Log("MOUSE OVER !");
        OnAnySingleTowerSelectUIHoveredEnter?.Invoke(this, new TowerData
        {
            towerInfos = correspondingTowerSO
        });
    }

    public void OnEventTrigger_PointerExit()
    {
        Debug.Log("Mouse exit");
        OnAnySingleTowerSelectUIHoveredExit?.Invoke(this, new TowerData
        {
            towerInfos = correspondingTowerSO
        });
    }

    public static void ResetStaticData()
    {
        OnAnySingleTowerSelectUIHoveredEnter = null;
        OnAnySingleTowerSelectUIHoveredExit = null;
        OnAnySingleTowerSelectUISelected = null;
    }
}
