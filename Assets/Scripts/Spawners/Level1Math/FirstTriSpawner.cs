using System;

namespace Spawners
{
    public class FirstTriSpawner : MathSpawnSO
    {
        public override int GetNumberMerdeToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 1.2) / 4);
        }

        public override int GetBigGuyToSpawn(int turn)
        {
            return 0; 
        }

        public override int GetDoggoToSpawn(int turn)
        {
            
            return (int)Math.Ceiling((turn * 0.7) / 3);
        }

        public override int GetSnipperToSpawn(int turn)
        {
            return (int)Math.Ceiling((turn * 0.5) / 3);
        }
    }
}