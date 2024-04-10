using System;
using UnityEngine.InputSystem.iOS.LowLevel;

namespace Spawners
{
    public class ThirdTriSpawner : IMathSpawn
    {
        int IMathSpawn.GetNumberMerdeToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 0.8) / 3);
        }

        int IMathSpawn.GetBigGuyToSpawn(int turn)
        {
            return (int) Math.Max(Math.Ceiling((turn - 1 * 0.5)), 0)  ;
        }

        int IMathSpawn.GetDoggoToSpawn(int turn)
        {
            return (int) Math.Round(turn * 0.7);
        }

        int IMathSpawn.GetSnipperToSpawn(int turn)
        {
            return 0;
        }
    }
}