using System;

namespace Spawners
{
    public class SecondTriSpawner : IMathSpawn
    {
        int IMathSpawn.GetNumberMerdeToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 0.8) / 4);
        }

        int IMathSpawn.GetBigGuyToSpawn(int turn)
        {
            return (int) Math.Max(Math.Ceiling((turn - 1 * 0.4)), 0)  ;
        }

        int IMathSpawn.GetDoggoToSpawn(int turn)
        {
            return 0;
        }

        int IMathSpawn.GetSnipperToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 0.25));
        }
    }
}