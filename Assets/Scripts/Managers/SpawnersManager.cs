using System.Collections.Generic;
using Utils;

namespace Managers
{
    public class SpawnersManager
    {
        public static SpawnersManager Instance { get; private set; }

        public SpawnersManager()
        {
            if (Instance != null) 
                Instance = this;
        }
        
        private List<Spawner> _spawners = new List<Spawner>();

        /// <summary>
        /// Ajoute un Spawner au manager, ! N'ajoute pas un spawner deja dans la liste...  
        /// </summary>
        /// <param name="spawnerToAdd"></param>
        public void AddSpawner(Spawner spawnerToAdd)
        {
            foreach (var spawner in _spawners)
            {
                if (spawner.Equals(spawnerToAdd)) ;
                {
                    return;
                }
            }
            _spawners.Add(spawnerToAdd);
        }
        
        /// <summary>
        /// Enleve un spawner du manager
        /// </summary>
        /// <param name="spawnerToRemove"></param>
        public void RemoveSpawner(Spawner spawnerToRemove)
        {
            foreach (var spawner in _spawners)
            {
                if (spawner == spawnerToRemove)
                {
                    _spawners.Remove(spawner);
                }
            }
        }

        /// <summary>
        /// Ajoute les spawners dans les creneaux
        /// </summary>
        public void AddSpawnersToTowerDefenseManager()
        {
            foreach (var spawner in _spawners)
            {
                if (!spawner.HasBeenAdded)
                {
                     
                    spawner.HasBeenAdded = true;
                }
            }  
        }
    }
}