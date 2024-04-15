using System;
using UnityEngine;

namespace Spawners
{
[CreateAssetMenu(menuName = "Math/ThirdTriSpawner")]
    public class ThirdTriSpawner : MathSpawnSO
    {
        public override int GetNumberMerdeToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 0.8) / 6);
        }

        public override int GetBigGuyToSpawn(int turn)
        {
            return (int) Math.Max(Math.Floor((turn - 1 * 0.2)), 0)  ;
        }

        public override int GetDoggoToSpawn(int turn)
        {
            return (int) Math.Floor(turn * 0.2);
        }

        public override int GetSnipperToSpawn(int turn)
        {
            return 0;
        }
    }
}