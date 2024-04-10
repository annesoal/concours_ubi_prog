using Spawners;
using UnityEngine;

namespace Grid.Blocks
{
    public class SpawnerBlock : BasicBlock 
    {
        [SerializeField] private IMathSpawn _mathSpawn;
        

        private int numberOfMerde;
        private int numberOfDoggos;
        private int numberOfBigGuy;
        private int numberOfSnipers;

        public void SetEnemiesToSpawn()
        {
           blockType =  
        }
        public void CalculateSpawnRate(int turn)
        {
            numberOfMerde = _mathSpawn.GetNumberMerdeToSpawn(turn);
            numberOfDoggos = _mathSpawn.GetDoggoToSpawn(turn);
            numberOfBigGuy = _mathSpawn.GetBigGuyToSpawn(turn);
            numberOfSnipers = _mathSpawn.GetSnipperToSpawn(turn);
        }

        public GameObject GetEnemyToSpawn()
        {
            if (numberOfMerde > 0)
            {
                numberOfMerde--;
                return;
            } 
            else if (numberOfDoggos > 0)
            {
                numberOfDoggos--;
                return;
            }
            else if (numberOfBigGuy > 0)
            {
                numberOfBigGuy--;
                return;
            }
            else if (numberOfSnipers > 0)
            {
                numberOfSnipers--;
                return;
            }
        }
    }
}