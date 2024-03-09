using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Managers
{
    public class SpawnersManager : MonoBehaviour
    {
        [Header("test")]
        [SerializeField] public List<Spawner> _listOfSpawners;  
        public static SpawnersManager Instance { get; private set; }

        public void Start()
        {
            if (Instance != null) 
                Instance = this;
        }
        
        /// <summary>
        /// Ajoute un Spawner au manager, ! N'ajoute pas un spawner deja dans la liste...  
        /// </summary>
        /// <param name="spawnerToAdd"></param>
        public void AddSpawner(Spawner spawnerToAdd)
        {
            foreach (var spawner in _listOfSpawners)
            {
                if (spawner.Equals(spawnerToAdd)) ;
                {
                    return;
                }
            }
           // _listOfSpawners.Add(spawnerToAdd);
        }
        
        /// <summary>
        /// Enleve un spawner du manager
        /// </summary>
        /// <param name="spawnerToRemove"></param>
        public void RemoveSpawner(Spawner spawnerToRemove)
        {
            foreach (var spawner in _listOfSpawners)
            {
                if (spawner.Equals(spawnerToRemove))
                {
                    _listOfSpawners.Remove(spawner);
                }
            }
        }

        /// <summary>
        /// Ajoute les spawners dans les creneaux
        /// </summary>
        public void AddSpawnersToTowerDefenseManager()
        {
            foreach (var spawner in _listOfSpawners)
            {
                TowerDefenseManager.Instance.OnCurrentStateChanged += spawner.AddSelfToTimeSlot;
            }  
        }
    }
}