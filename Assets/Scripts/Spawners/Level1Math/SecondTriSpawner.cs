using System;
using UnityEngine;

namespace Spawners
{
    [CreateAssetMenu(menuName = "Math/SecondTriSpawner")]
    public class SecondTriSpawner : MathSpawnSO
    {
        public override int GetNumberMerdeToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 0.8) / 4);
        }

        public override int GetBigGuyToSpawn(int turn)
        {
            return (int) Math.Max(Math.Ceiling((turn - 1 * 0.4)), 0)  ;
        }

        public override int GetDoggoToSpawn(int turn)
        {
            return 0;
        }

        public override int GetSnipperToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 0.25));
        }
    }
}