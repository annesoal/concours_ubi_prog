using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleBuildableObjectSelectButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public event EventHandler OnButtonSelectedByController ;
    
    public void OnSelect(BaseEventData eventData)
    {
        OnButtonSelectedByController?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnButtonDeselectedByController ;
    
    public void OnDeselect(BaseEventData eventData)
    {
        OnButtonDeselectedByController?.Invoke(this, EventArgs.Empty);
    }
}
