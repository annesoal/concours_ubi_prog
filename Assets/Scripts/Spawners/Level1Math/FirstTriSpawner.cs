using System;
using UnityEngine;

namespace Spawners
{
[CreateAssetMenu(menuName = "Math/FirstTriSpawner")]
    public class FirstTriSpawner : MathSpawnSO
    {
        public override int GetNumberMerdeToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 1.2) / 8);
        }

        public override int GetBigGuyToSpawn(int turn)
        {
            return 0; 
        }

        public override int GetDoggoToSpawn(int turn)
        {
            
            return (int)Math.Floor(turn *0.3);
        }

        public override int GetSnipperToSpawn(int turn)
        {
            return 0;
        }
    }
}