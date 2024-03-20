using System;
using System.Collections;
using System.Collections.Generic;
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
            IEnumerator coroutineEnvTurn = EnvironmentTurn(TowerDefenseManager.Instance.EnergyAvailable);
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
            IAManager.MoveEnemies(totalEnergy);
            yield return new WaitForSeconds(0.5f);
            totalEnergy--;
        }
        OnEnvironmentTurnEnded?.Invoke(this, EventArgs.Empty);
        yield return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="totalEnergy">Énergie qui devra être dépensée avant de retourner à la pause tactique.</param>
   // public void EnableEnvironmentTurn(int totalEnergy)
   // {
   //     // NOTE : AIM = AI Manager
   //     // AIM.ComputePaths()
   //     PreparePlayers();
   //      
   //     while (HasEnergyLeft(totalEnergy))
   //     {
   //         MovePlayers();
   //         
   //         // AIM.PlaySubordinatesTurn();
   //         
   //         // TowerManager.PlayTowersTurn();
   //         IAManager.MoveEnemies(totalEnergy);
   //         totalEnergy--;
   //     }
   //     
   //         
   //     OnEnvironmentTurnEnded?.Invoke(this, EventArgs.Empty);
   // }

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
