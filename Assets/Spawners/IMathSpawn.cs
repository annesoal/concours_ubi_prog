namespace Spawners
{
    public interface IMathSpawn
    {
        protected int GetNumberMerdeToSpawn(int turn); 
        protected int GetBigGuyToSpawn(int turn); 
        protected int GetDoggoToSpawn(int turn); 
        protected int GetSnipperToSpawn(int turn); 
    }
}
