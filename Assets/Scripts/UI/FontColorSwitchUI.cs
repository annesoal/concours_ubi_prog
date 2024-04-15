using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FontColorSwitchUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TextMeshProUGUI textToSwitch;
    
    [Header("Override Colors")]
    [SerializeField] private bool useOverrideColor;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color selectedColor;
    
    public void OnSelect(BaseEventData eventData)
    {
        if (useOverrideColor)
        {
            textToSwitch.color = selectedColor;
        }
        else
        {
            textToSwitch.color = ColorPaletteUI.Instance.ColorPaletteSo.lightBackgroundTextColor;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (useOverrideColor)
        {
            textToSwitch.color = selectedColor;
        }
        else
        {
            textToSwitch.color = ColorPaletteUI.Instance.ColorPaletteSo.darkBackgroundTextColor;
        }
    }
}
