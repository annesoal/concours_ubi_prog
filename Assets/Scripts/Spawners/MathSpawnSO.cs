using UnityEngine;

namespace Spawners
{
    public abstract class MathSpawnSO : ScriptableObject
    {
        public abstract int GetNumberMerdeToSpawn(int turn); 
        public abstract int GetBigGuyToSpawn(int turn); 
        public abstract int GetDoggoToSpawn(int turn); 
        public abstract int GetSnipperToSpawn(int turn); 
    }
}
