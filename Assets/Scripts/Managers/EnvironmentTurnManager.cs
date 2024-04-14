using System;
using System.Collections;
using Enemies;
using Grid;
using Managers;
using Unity.Netcode;
using UnityEngine;

public class EnvironmentTurnManager : MonoBehaviour
{
    public static EnvironmentTurnManager Instance;
    public event EventHandler OnEnvironmentTurnEnded;
    public int Turn
    {
        get => _turn;
        set => _turn = value;
    }
    private int _turn; 
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
            IEnumerator coroutineEnvTurn = EnvironmentTurn(TowerDefenseManager.Instance.IsFirstTurn);
            StartCoroutine(coroutineEnvTurn);
        }
    }

    private IEnumerator EnvironmentTurn(bool isFirstTurn)
    {
        PreparePlayers();
        if (!isFirstTurn)
        {
            int totalEnergyPlayers = Player.Energy;
            
            while (HasEnergyLeft(totalEnergyPlayers))
            {
                MovePlayers();
                yield return new WaitUntil(ReadyToMoveNPCs);
                totalEnergyPlayers--;
                ResetPlayerReadyCount();
            }
            
            int NPCEnergy = Enemy.Energy; 
            
            EnemySpawnerManager.Instance.StartMathSpawners(_turn);
            while (HasEnergyLeft(NPCEnergy))
            {
                //Debug.Log("EVM Avant le spawn");
                EnemySpawnerManager.Instance.Spawn(_turn);
                 
                //Debug.Log("EVM avant play tower in game turn");
                StartCoroutine(BaseTower.PlayTowersInGameTurn());
                yield return new WaitUntil(BaseTower.HasFinishedTowersTurn);
                
                //Debug.Log("EVM avant move enemies");
                IAManager.Instance.BackendMoveEnemies();
                StartCoroutine(IAManager.Instance.MoveEnemies());
                yield return new WaitUntil(IAManager.Instance.hasMovedEnemies);
                
                //Debug.Log("Fin Iteration boucle EVM");
                NPCEnergy--;
            }
            //Debug.Log("Sortie de la boucle EVM");
        
            IAManager.ResetEnemies();
        
            _turn++;
        }
        
        yield return new WaitForSeconds(0.05f);    
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
