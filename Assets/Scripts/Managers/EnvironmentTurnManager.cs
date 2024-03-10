using System;
using UnityEngine;

namespace Managers
{
    public class EnvironmentTurnManager : MonoBehaviour
    {
        public static EnvironmentTurnManager Instance;
    
        public event EventHandler OnEnvironmentTurnEnded;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Instance = this;
        }

        /// <summary>
        /// </summary>
        /// <param name="totalEnergy">Énergie qui devra être dépensée avant de retourner à la pause tactique.</param>
        public void EnableEnvironmentTurn(int totalEnergy)
        {
            Debug.Log("EnvTurn");
            // NOTE : AIM = AI Manager
            // AIM.ComputePaths()
        
            while (HasEnergyLeft(totalEnergy))
            {
                // MovePlayers
            
                // AIM.PlaySubordinatesTurn();
            
                // TowerManager.PlayTowersTurn();
            
                totalEnergy--;
            }
            OnEnvironmentTurnEnded?.Invoke(this, EventArgs.Empty);
        }

        private bool HasEnergyLeft(int energyLeft)
        {
            return energyLeft > 0;
        }
    }
}
