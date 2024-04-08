using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class GameStateUI : MonoBehaviour
{
    [SerializeField] private GameObject currentStateBorder;
    [SerializeField] private TextMeshProUGUI roundsLeftText;
    
    [Header("Timer")]
    [SerializeField] private GameObject timerGameObject;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private bool _canUpdateTimerText = false;
    
    private void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        TowerDefenseManager.Instance.OnRoundNumberIncreased += TowerDefenseManager_OnRoundNumberIncreased;
        
        BasicShowHide.Hide(gameObject);
    }

    private void Update()
    {
        if (_canUpdateTimerText)
        {
            timerText.text = "" + Mathf.FloorToInt(TowerDefenseManager.Instance.TacticalPauseTimer);
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
    
    private void TowerDefenseManager_OnRoundNumberIncreased(object sender, EventArgs e)
    {
        int roundsLeft = TowerDefenseManager.Instance.totalRounds - TowerDefenseManager.Instance.currentRoundNumber;
        roundsLeftText.text ="Rounds left: " + roundsLeft;
    }

    private void DisplayEnvironmentTurnStateUI()
    {
        BasicShowHide.Hide(currentStateBorder);
        
        BasicShowHide.Show(gameObject);
        
        _canUpdateTimerText = false;
        BasicShowHide.Hide(timerGameObject);
    }
    
    private void DisplayTacticalPauseStateUI()
    {
        BasicShowHide.Hide(currentStateBorder);
        
        BasicShowHide.Show(gameObject);
        
        _canUpdateTimerText = true;
        BasicShowHide.Show(timerGameObject);
    }
}
