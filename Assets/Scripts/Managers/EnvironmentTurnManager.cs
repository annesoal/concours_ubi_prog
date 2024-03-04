using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTurnManager : MonoBehaviour
{
    public static EnvironmentTurnManager Instance;
    
    public event EventHandler OnEnvironmentTurnEnded;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// </summary>
    /// <param name="totalEnergy">Énergie qui devra être dépensée avant de retourner à la pause tactique.</param>
    public void EnableEnvironmentTurn(int totalEnergy)
    {
        /**
         * Effecuter le Spawn d'enemis (prévu avant, consultable par les joueurs)
         *
         * AIM.ComputePaths()
         * while (hasEnergy())
         * {
         *     AIM.PlaySubordinatesTurn();
         *
         *     TowerManager.PlayTowersTurn();
         *
         *     currentEnvironmentEnergy--;
         * }
         *
         * currentEnvironmentEnergy = INITIAL_CURRENT_ENERGY;
         * GoToSpecifiedState(State.TacticalPause);
         */
    }
}
