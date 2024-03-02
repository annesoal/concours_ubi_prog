using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class StartCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;

    private void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        
        BasicShowHide.Hide(gameObject);
    }

    private void Update()
    {
        DisplayCountDown();
    }

    private int _previousCountDownNumber = 0;
    
    private void DisplayCountDown()
    {
        float currentValue = TowerDefenseManager.Instance.CountDownToStartTimer;

        int currentValueCeiled = Mathf.CeilToInt(currentValue);

        countDownText.text = currentValueCeiled.ToString();

        if (_previousCountDownNumber != currentValueCeiled)
        {
            _previousCountDownNumber = currentValueCeiled;
        }
    }

    private void TowerDefenseManager_OnCurrentStateChanged
        (object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == TowerDefenseManager.State.CountdownToStart)
        {
            BasicShowHide.Show(gameObject);
        }
        else
        {
            BasicShowHide.Hide(gameObject);
        }
    }
}
