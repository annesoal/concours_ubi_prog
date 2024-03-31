using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class EnvironmentTurnManager : MonoBehaviour
{
    public static EnvironmentTurnManager Instance;
    
    public event EventHandler OnEnvironmentTurnEnded;
    public bool PlayerHasBeenMoved { private get; set; }
    
    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
       TowerDefenseManager.Instance.OnCurrentStateChanged += ListenForEnvironmentTurn; 
    }

    private void ListenForEnvironmentTurn(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == TowerDefenseManager.State.EnvironmentTurn)
        {
            
            IEnumerator coroutineEnvTurn = EnvironmentTurn(TowerDefenseManager.Instance.energyToUse);
            StartCoroutine(coroutineEnvTurn);
        }
    }

    private IEnumerator EnvironmentTurn(int totalEnergy)
    {
        PreparePlayers();
        
        while (HasEnergyLeft(totalEnergy))
        {   
            MovePlayers();
            yield return new WaitForSeconds(0.5f);
            
            // AIM.PlaySubordinatesTurn();
            
            StartCoroutine(BaseTower.PlayTowersInGameTurn());
            // Play Trap turn
            yield return new WaitUntil(BaseTower.HasFinishedTowersTurn);
            IAManager.MoveEnemies(totalEnergy);
            yield return new WaitForSeconds(0.5f);
            totalEnergy--;
        }

        IAManager.ResetEnemies();

        yield return new WaitForSeconds(0.01f);
        OnEnvironmentTurnEnded?.Invoke(this, EventArgs.Empty);
    }

    private bool HasEnergyLeft(int energyLeft)
    {
        return energyLeft > 0;
    }

    private void MovePlayers()
    {
        GameMultiplayerManager.Instance.MovePlayersClientRpc();
    }

    private void PreparePlayers()
    {
       GameMultiplayerManager.Instance.PreparePlayersClientRpc(); 
    }
}
