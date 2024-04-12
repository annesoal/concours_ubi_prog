using System;
using UnityEngine;

namespace Spawners
{
[CreateAssetMenu(menuName = "Math/ThirdTriSpawner")]
    public class ThirdTriSpawner : MathSpawnSO
    {
        public override int GetNumberMerdeToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 0.8) / 3);
        }

        public override int GetBigGuyToSpawn(int turn)
        {
            return (int) Math.Max(Math.Ceiling((turn - 1 * 0.5)), 0)  ;
        }

        public override int GetDoggoToSpawn(int turn)
        {
            return (int) Math.Round(turn * 0.7);
        }

        public override int GetSnipperToSpawn(int turn)
        {
            return 0;
        }
    }
}