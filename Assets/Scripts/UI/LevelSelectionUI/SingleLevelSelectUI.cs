using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleLevelSelectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform amuletLayout;
    
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
}
