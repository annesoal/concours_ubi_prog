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
        
        public void PlaceObjects(Vector2Int[] positionToObstacles, int indexInGameObjectList)
        {
            foreach (Vector2Int positionOfSpawn in positionToObstacles)
            {
                GameObject instance = Instantiate(_gameObjectsToSpawn[indexInGameObjectList]);
                instance.GetComponent<NetworkObject>().Spawn(true);
                
                TilingGrid.grid.PlaceObjectAtPositionOnGrid(instance, positionOfSpawn);
            }
        }
        
        /// <summary>
        /// Place les objets dans le server et le client
        /// </summary>
        /// <param name="positionToObstacles"> positions des objets </param>
        /// <param name="positionInGameObjectList"> position de l'objet a spawn dans la list des objets</param>
        [ClientRpc]
        public void PlaceObjectsClientRpc(Vector2Int[] positionToObstacles, int positionInGameObjectList)
        {
            List<Vector2Int> positions = positionToObstacles.ToList();
            Spawner.InstantiateObstacles(positions, _gameObjectsToSpawn.ElementAt(positionInGameObjectList));
       
        }
    }
}