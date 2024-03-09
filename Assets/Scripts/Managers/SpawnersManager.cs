using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Managers
{
    public class SpawnersManager : NetworkBehaviour
    {
        [Header("test")] [SerializeField] public List<Spawner> listOfSpawners;

        private List<GameObject> _gameObjectsToSpawn = new List<GameObject>();
        public static SpawnersManager Instance { get; private set; }

        private SpawnersManager()
        {
          
              
            
        }
        public void Start()
        {
            Instance = this;
            Debug.Log("Instance has been added");   
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

        private void FillListOfGameObjectsToSpawn()
        {
            foreach (var spawner in listOfSpawners)
            {
                _gameObjectsToSpawn.Add(spawner.ObjectToSpawn);
              
            } 
        }
        
        [ClientRpc]
        public void GenerateObjectsClientRpc(Vector2Int[] positionToObstacles, int positionInList)
        {
            List<Vector2Int> positions = positionToObstacles.ToList();
            Spawner.InstantiateObstacles(positions, _gameObjectsToSpawn.ElementAt(positionInList));
       
        }
    }
}