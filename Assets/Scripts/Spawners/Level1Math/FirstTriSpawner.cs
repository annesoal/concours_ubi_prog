using System;
using Managers;
using UnityEngine;

namespace Spawners
{
[CreateAssetMenu(menuName = "Math/FirstTriSpawner")]
    public class FirstTriSpawner : MathSpawnSO
    {
        public override int GetNumberMerdeToSpawn(int turn)
        {
            if (turn > EnemySpawnerManager.TotalRounds)
                return 0; 
            return (int)Math.Ceiling((turn * 1.2) / 8);
        }

        public override int GetBigGuyToSpawn(int turn)
        {
            if (turn > EnemySpawnerManager.TotalRounds)
                return 0; 
            return 0; 
        }

        public override int GetDoggoToSpawn(int turn)
        {
            
            if (turn > EnemySpawnerManager.TotalRounds)
                return 0; 
            return (int)Math.Floor(turn *0.3);
        }

        public override int GetSnipperToSpawn(int turn)
        {
            if (turn > EnemySpawnerManager.TotalRounds)
                return 0; 
            return 0;
        }
    }
}