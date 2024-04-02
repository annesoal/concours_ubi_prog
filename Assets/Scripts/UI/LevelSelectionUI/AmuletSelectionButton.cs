using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using UnityEngine;
using UnityEngine.EventSystems;

public class AmuletSelectionButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private AmuletSO _associatedAmuletSO;

    public static event EventHandler<OnAmuletButtonSelectedEventArgs> OnAmuletButtonSelected;
    public class OnAmuletButtonSelectedEventArgs : EventArgs
    {
        public AmuletSO AmuletSo;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        OnAmuletButtonSelected?.Invoke(this, new OnAmuletButtonSelectedEventArgs
        {
            AmuletSo = _associatedAmuletSO,
        });
    }

    public static event EventHandler<OnAmuletButtonDeselectedEventArgs> OnAmuletButtonDeselected;
    public class OnAmuletButtonDeselectedEventArgs : EventArgs
    {
        public AmuletSO AmuletSo;
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        OnAmuletButtonDeselected?.Invoke(this, new OnAmuletButtonDeselectedEventArgs
        {
            AmuletSo = _associatedAmuletSO,
        });
    }

    public void SetAssociatedAmuletSO(AmuletSO amuletSO)
    {
        _associatedAmuletSO = amuletSO;
    }

    public static void ResetStaticData()
    {
        OnAmuletButtonSelected = null;
        OnAmuletButtonDeselected = null;
    }
}
