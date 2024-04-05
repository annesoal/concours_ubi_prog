using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Managers
{
    public class SpawnersManager : NetworkBehaviour
    {
        [Header("Spawners")] [SerializeField] public List<Spawner> listOfSpawners;

        private List<GameObject> _gameObjectsToSpawn = new List<GameObject>();
        public static SpawnersManager Instance { get; private set; }

        public void Start()
        {
            Instance = this;
            AddSpawnersToTowerDefenseManager();
            FillListOfGameObjectsToSpawn();
        }
    
        /// <summary>
        /// Ajoute les spawners dans les creneaux
        /// </summary>
        private void AddSpawnersToTowerDefenseManager()
        {
            int position = 0;
            foreach (var spawner in listOfSpawners)
            {
                spawner.Initialize(IsServer, position);
                TowerDefenseManager.Instance.OnCurrentStateChanged += spawner.AddSelfToTimeSlot;
                position++;
              
            }  
        }

        /// <summary>
        /// Met les objets de chaques Spawners dans une liste
        /// </summary>
        private void FillListOfGameObjectsToSpawn()
        {
            foreach (var spawner in listOfSpawners)
            {
                _gameObjectsToSpawn.Add(spawner.ObjectToSpawn);
              
            } 
        }
        
        public void PlaceObjects(Vector2Int[] positionToObstacles, int indexInGameObjectList, Func<Cell, bool> isInvalidCell)
        {
            foreach (Vector2Int positionOfSpawn in positionToObstacles)
            {
                Cell cell = TilingGrid.grid.GetCell(positionOfSpawn);
                    if (isInvalidCell.Invoke(cell))
                        continue; 
                GameObject instance = Instantiate(_gameObjectsToSpawn[indexInGameObjectList]);
                TilingGrid.grid.PlaceObjectAtPositionOnGridOffline(instance, positionOfSpawn);
                instance.GetComponent<NetworkObject>().Spawn(true);
            }
        }
    }
}