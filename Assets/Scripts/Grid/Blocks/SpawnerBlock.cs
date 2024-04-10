using System;
using Spawners;
using Unity.Networking.Transport;
using UnityEngine;

namespace Grid.Blocks
{
    public class SpawnerBlock : BasicBlock 
    {
        [SerializeField] private IMathSpawn _mathSpawn;

        [SerializeField] public Transform positionToSpawn;
        
        private GameObject _merde; 
        private GameObject _doggo; 
        private GameObject _bigGuy; 
        private GameObject _sniper; 

        private int numberOfMerde;
        private int numberOfDoggos;
        private int numberOfBigGuy;
        private int numberOfSnipers;

        public void Initialize()
        {
            blockType = BlockType.EnemySpawnBlock;
        }
        public void SetEnemiesToSpawn(ListEnemiesToSpawnSO list)
        {
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
                return _merde;
            } 
            else if (numberOfDoggos > 0)
            {
                numberOfDoggos--;
                return _doggo;
            }
            else if (numberOfBigGuy > 0)
            {
                numberOfBigGuy--;
                return _bigGuy;
            }
            else if (numberOfSnipers > 0)
            {
                numberOfSnipers--;
                return _sniper;
            }

            throw new Exception("wtf");
        }
    }
}