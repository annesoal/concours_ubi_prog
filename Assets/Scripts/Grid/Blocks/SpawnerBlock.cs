using System;
using Spawners;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grid.Blocks
{
    public class SpawnerBlock : BasicBlock 
    {
        [FormerlySerializedAs("_mathSpawn")] public MathSpawnSO mathSpawnSo;

        [SerializeField] public Transform positionToSpawn;
        [SerializeField] private ListEnemiesToSpawnSO list;
        
        private GameObject _merde; 
        private GameObject _doggo; 
        private GameObject _bigGuy; 
        private GameObject _sniper; 

        private int numberOfMerde;
        private int numberOfDoggos;
        private int numberOfBigGuy;
        private int numberOfSnipers;

        public void Awake()
        {
            blockType = BlockType.EnemySpawnBlock;
            SetEnemiesToSpawn(list);
        }
        public void SetEnemiesToSpawn(ListEnemiesToSpawnSO list)
        {
            this._merde = list.Merde;
            this._doggo = list.Doggo;
            this._bigGuy = list.BigGuy;
            this._sniper = list.Sniper;
        }
        public void CalculateSpawnRate(int turn)
        {
            numberOfMerde = mathSpawnSo.GetNumberMerdeToSpawn(turn);
            numberOfDoggos = mathSpawnSo.GetDoggoToSpawn(turn);
            numberOfBigGuy = mathSpawnSo.GetBigGuyToSpawn(turn);
            numberOfSnipers = mathSpawnSo.GetSnipperToSpawn(turn);
        }

        public GameObject GetEnemyToSpawn()
        {
            if (numberOfSnipers > 0)
            {
                numberOfSnipers--;
                return _sniper;
            }
            
            //if (numberOfMerde > 0)
            //{
            //    numberOfMerde--;
            //    return _merde;
            //} 
            //else if (numberOfDoggos > 0)
            //{
            //    numberOfDoggos--;
            //    return _doggo;
            //}
            //else if (numberOfBigGuy > 0)
            //{
            //    numberOfBigGuy--;
            //    return _bigGuy;
            //}
            //else if (numberOfSnipers > 0)
            //{
            //    numberOfSnipers--;
            //    return _sniper;
            //}

            return null;
        }
    }
}