using System;

namespace Spawners
{
    public class FirstTriSpawner : IMathSpawn
    {
        int IMathSpawn.GetNumberMerdeToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 1.2) / 4);
        }

        int IMathSpawn.GetBigGuyToSpawn(int turn)
        {
            return 0; 
        }

        int IMathSpawn.GetDoggoToSpawn(int turn)
        {
            
            return (int)Math.Ceiling((turn * 0.7) / 3);
        }

        int IMathSpawn.GetSnipperToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 0.5) / 3);
        }
    }
}