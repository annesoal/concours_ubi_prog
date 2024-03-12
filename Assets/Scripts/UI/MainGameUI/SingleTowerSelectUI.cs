using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleTowerSelectUI : MonoBehaviour
{
    [SerializeField] private Button selectionButton;

    public void SetIcon(Sprite icon)
    {
        selectionButton.image.sprite = icon;
    }

    public void OnEventTrigger_PointerEnter()
    {
        Debug.Log("MOUSE OVER !");
    }

    public void OnEventTrigger_PointerExit()
    {
        Debug.Log("Mouse exit");
    }
}
