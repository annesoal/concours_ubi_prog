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
    
    [Header("Energy")]
    [SerializeField] private GameObject energyUI;
    [SerializeField] private TextMeshProUGUI energyLeftText;
    
    [Header("Enemy or player turn indicator text")]
    [SerializeField] private TextMeshProUGUI enemyPlayerTurnText;
    [SerializeField] private Color playerTextColor;
    [SerializeField] private Color enemyTextColor;
    
    private bool _canUpdateTimerText = false;
    
    private void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
        TowerDefenseManager.Instance.OnRoundNumberIncreased += TowerDefenseManager_OnRoundNumberIncreased;

        StartCoroutine(ConnectPlayerEventOnSpawn());
    }

    private IEnumerator ConnectPlayerEventOnSpawn()
    {
        while (Player.LocalInstance is null || !Player.LocalInstance)
        {
            yield return new WaitForSeconds(0.8f);
        }
        
        Player.LocalInstance.OnPlayerEnergyChanged += PlayerLocalInstance_OnPlayerEnergyChanged;
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
        int roundsLeft = TowerDefenseManager.Instance.currentRoundNumber;
        roundsLeftText.text ="Rounds : " + roundsLeft;
    }

    private const string ENEMY_TURN_TEXT = "ENEMY TURN";
    
    private void DisplayEnvironmentTurnStateUI()
    {
        enemyPlayerTurnText.text = ENEMY_TURN_TEXT;
        enemyPlayerTurnText.color = enemyTextColor;
        
        BasicShowHide.Show(currentStateBorder);
        
        BasicShowHide.Show(gameObject);
        
        _canUpdateTimerText = false;
        //BasicShowHide.Hide(timerGameObject);
    }

    private const string PLAYER_TURN_TEXT = "PLAYER TURN";
        
    private void DisplayTacticalPauseStateUI()
    {
        enemyPlayerTurnText.text = PLAYER_TURN_TEXT;
        enemyPlayerTurnText.color = playerTextColor;
        
        BasicShowHide.Hide(currentStateBorder);
        
        BasicShowHide.Show(gameObject);
        
        _canUpdateTimerText = true;
        //BasicShowHide.Show(timerGameObject);
    }

    private int _currentEnergyTweenId;
    private void PlayerLocalInstance_OnPlayerEnergyChanged(object sender, Player.OnPlayerEnergyChangedEventArgs e)
    {
        if (HasLowEnergy(e.Energy))
        {
            energyLeftText.color = Color.red;
        }
        else if (HasMediumEnergy(e.Energy))
        {
            energyLeftText.color = ColorPaletteUI.Instance.ColorPaletteSo.errorColor;
        }
        else
        {
            energyLeftText.color = ColorPaletteUI.Instance.ColorPaletteSo.lightBackgroundTextColor;
        }
        
        energyUI.transform.localScale = Vector3.one;
        
        energyLeftText.text = "" +  e.Energy;

        LeanTween.cancel(_currentEnergyTweenId);
        _currentEnergyTweenId = energyUI.transform.LeanScale(Vector3.one * 1.1f, 0.2f).setEaseOutCirc().setLoopPingPong(1).id;
    }

    private bool HasLowEnergy(int currentEnergy)
    {
        return currentEnergy <= Player.Energy / 5;
    }
    
    private bool HasMediumEnergy(int currentEnergy)
    {
        return currentEnergy <= Player.Energy / 2;
    }
}
