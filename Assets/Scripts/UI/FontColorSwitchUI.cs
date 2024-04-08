using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FontColorSwitchUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TextMeshProUGUI textToSwitch;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color selectedColor;
    
    public void OnSelect(BaseEventData eventData)
    {
        textToSwitch.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        textToSwitch.color = baseColor;
    }
}
