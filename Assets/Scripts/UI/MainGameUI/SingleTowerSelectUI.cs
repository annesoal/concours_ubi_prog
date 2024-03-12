using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleTowerSelectUI : MonoBehaviour
{
    [SerializeField] private Button selectionButton;

    private TowerSO correspondingTowerSO;

    public void SetCorrespondingTowerSO(TowerSO toSet)
    {
        correspondingTowerSO = toSet;
        
        SetIcon(toSet.towerIcon);
    }
    
    public void SetIcon(Sprite icon)
    {
        selectionButton.image.sprite = icon;
    }

    public static event EventHandler<TowerData> OnAnySingleTowerSelectUIHoveredEnter;
    public static event EventHandler<TowerData> OnAnySingleTowerSelectUIHoveredExit;

    public class TowerData : EventArgs
    {
        public TowerSO towerInfos;
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
    }
}
