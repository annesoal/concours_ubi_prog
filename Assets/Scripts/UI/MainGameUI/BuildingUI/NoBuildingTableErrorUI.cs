using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class NoBuildingTableErrorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;
    
    [Header("Tweening")]
    [SerializeField] private float tweeningTime;

    private LTDescr _currentTweening = null;

    private void Start()
    {
        errorText.color = ColorPaletteUI.Instance.ColorPaletteSo.errorColor;
        
        BasicShowHide.Hide(gameObject);
    }

    public void Show()
    {
        if (_currentTweening == null)
        {
            transform.localScale = Vector3.zero;
        
            BasicShowHide.Show(gameObject);

            _currentTweening = transform.LeanScale(Vector3.one, tweeningTime).setEaseOutExpo().setLoopPingPong(1).setOnComplete(Hide);
        }
    }

    private void Hide()
    {
        BasicShowHide.Hide(gameObject);
        
        _currentTweening = null;
    }
}
