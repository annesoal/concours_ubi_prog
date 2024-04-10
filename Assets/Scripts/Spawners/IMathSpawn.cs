namespace Spawners
{
    public interface IMathSpawn
    {
        public int GetNumberMerdeToSpawn(int turn); 
        public int GetBigGuyToSpawn(int turn); 
        public int GetDoggoToSpawn(int turn); 
        public int GetSnipperToSpawn(int turn); 
    }
}
