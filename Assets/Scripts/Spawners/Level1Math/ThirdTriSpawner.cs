using System;
using UnityEngine;

namespace Spawners
{
[CreateAssetMenu(menuName = "Math/ThirdTriSpawner")]
    public class ThirdTriSpawner : MathSpawnSO
    {
        public override int GetNumberMerdeToSpawn(int turn)
        {
            if (turn <= 5)
            {
                return (int)Math.Ceiling((turn * 0.8) / 6);
            }
            else
            {
                return 0;
            }
        }

        public override int GetBigGuyToSpawn(int turn)
        {
            if (turn <= 5)
            {
                return (int) Math.Max(Math.Floor((turn - 1 * 0.2)), 0)  ;
            }
            else
            {
                return 0;
            }
        }

        public override int GetDoggoToSpawn(int turn)
        {
            if (turn <= 5)
            {
                return (int) Math.Floor(turn * 0.2);
            }
            else
            {
                return 0;
            }
        }

        public override int GetSnipperToSpawn(int turn)
        {
            return 0;
        }
    }
}