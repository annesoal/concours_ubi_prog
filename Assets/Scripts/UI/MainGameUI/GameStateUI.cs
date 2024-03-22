using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class GameStateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentStateText;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private bool _canUpdateTimerText = false;
    
    private void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        
        BasicShowHide.Hide(gameObject);
    }

    private void Update()
    {
        if (_canUpdateTimerText)
        {
            timerText.text = "Time left : " + Mathf.FloorToInt(TowerDefenseManager.Instance.TacticalPauseTimer);
        }
    }

    private void TowerDefenseManager_OnCurrentStateChanged(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        switch (e.newValue)
        {
            case TowerDefenseManager.State.EnvironmentTurn:
                DisplayEnvironmentTurnStateUI();
                break;
            case TowerDefenseManager.State.TacticalPause:
                DisplayTacticalPauseStateUI();
                break;
            default:
                BasicShowHide.Hide(gameObject);
                break;
        }
    }

    private void DisplayEnvironmentTurnStateUI()
    {
        currentStateText.text = "ENVIRONMENT TURN";
        
        BasicShowHide.Show(gameObject);
        
        _canUpdateTimerText = false;
        BasicShowHide.Hide(timerText.gameObject);
    }
    
    private void DisplayTacticalPauseStateUI()
    {
        currentStateText.text = "TACTICAL PAUSE";
        
        BasicShowHide.Show(gameObject);
        
        _canUpdateTimerText = true;
        BasicShowHide.Show(timerText.gameObject);
    }
}
