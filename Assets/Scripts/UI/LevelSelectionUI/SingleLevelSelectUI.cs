using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleLevelSelectUI : MonoBehaviour
{
    // TODO levelSelectSO
    
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
}
