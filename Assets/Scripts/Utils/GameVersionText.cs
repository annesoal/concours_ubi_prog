using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameVersionText : MonoBehaviour
{
    private TextMeshProUGUI _textComponent;

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();

        _textComponent.text = "TNT VERSION : " + Application.version;
    }
    
}
