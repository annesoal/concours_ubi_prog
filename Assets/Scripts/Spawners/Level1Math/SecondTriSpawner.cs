using System;
using UnityEngine;

namespace Spawners
{
    [CreateAssetMenu(menuName = "Math/SecondTriSpawner")]
    public class SecondTriSpawner : MathSpawnSO
    {
        public override int GetNumberMerdeToSpawn(int turn)
        {
            if (turn <= 5)
            {
                return (int)Math.Floor(turn * 0.18);
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
                return (int)(Math.Floor((turn * 0.1)))  ;
            }
            else
            {
                return 0;
            }
        }

        public override int GetDoggoToSpawn(int turn)
        {
            return 0;
        }

        public override int GetSnipperToSpawn(int turn)
        {
            if (turn <= 5)
            {
                return (int)Math.Ceiling((turn * 0.1));
            }
            else
            {
                return 0;
            }
        }
    }
}