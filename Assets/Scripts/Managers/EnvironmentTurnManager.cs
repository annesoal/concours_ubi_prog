using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Managers;
using Unity.Netcode;
using UnityEngine;

public class EnvironmentTurnManager : MonoBehaviour
{
    public static EnvironmentTurnManager Instance;

    public static int EnemyEnergy = 0;
    public event EventHandler OnEnvironmentTurnEnded;
    public bool PlayerHasBeenMoved { private get; set; }

    private static int _turn = -1;
    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            TowerDefenseManager.Instance.OnCurrentStateChanged += ListenForEnvironmentTurn;
        }
    }

    private void ListenForEnvironmentTurn(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == TowerDefenseManager.State.EnvironmentTurn)
        {
            TilingGrid.grid.ClearAllClientTopOfCells();
            IEnumerator coroutineEnvTurn = EnvironmentTurn(TowerDefenseManager.Instance.energyToUse);
            StartCoroutine(coroutineEnvTurn);
        }
    }

    private IEnumerator EnvironmentTurn(int totalEnergy)
    {
        PreparePlayers();
        int totalEnergyPlayers = totalEnergy;
        while (HasEnergyLeft(totalEnergyPlayers))
        {
            MovePlayers();
            yield return new WaitUntil(ReadyToMoveNPCs);
            totalEnergyPlayers--;
            ResetPlayerReadyCount();
        }
        // Discussion avec Malo pour decoupler l'energie des 2 groupes
        int NPCEnegy = EnemyEnergy; 
        
        EnemySpawnerManager.Instance.StartMathSpawners(_turn);
        while (HasEnergyLeft(NPCEnegy))
        {
            Debug.Log("EVM Avant le spawn");
            EnemySpawnerManager.Instance.Spawn(_turn);
            
            Debug.Log("EVM avant play tower in game turn");
            StartCoroutine(BaseTower.PlayTowersInGameTurn());
            yield return new WaitUntil(BaseTower.HasFinishedTowersTurn);
            
            Debug.Log("EVM avant move enemies");
            StartCoroutine(IAManager.Instance.MoveEnemies(totalEnergy));
            yield return new WaitUntil(IAManager.Instance.hasMovedEnemies);
            
            Debug.Log("Fin Iteration boucle EVM");
            NPCEnegy--;
        }
        Debug.Log("Sortie de la boucle EVM");

        IAManager.ResetEnemies();

        _turn++;
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

    private int playersFinishedMoving = 0;

    public void IncrementPlayerFinishedMoving()
    {
        playersFinishedMoving++;
    }

    private bool ReadyToMoveNPCs()
    {
        return playersFinishedMoving >= 2;
    }

    private void ResetPlayerReadyCount()
    {
        playersFinishedMoving = 0;
    }
}