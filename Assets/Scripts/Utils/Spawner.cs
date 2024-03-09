using UnityEngine;

namespace Utils
{
    public abstract class Spawner
    {
        private GameObject _objectToSpawn;
        private bool _hasBeenAdded = false;
        public bool HasBeenAdded
        {
            get
            {
                return _hasBeenAdded;
            }
            set
            {
                _hasBeenAdded = value;
            }
        }

        public Spawner(GameObject objectToSpawn, TowerDefenseManager.State[] state)
        {
            _objectToSpawn = objectToSpawn;
        }

        public abstract Vector2Int[] GeneratePosition();
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            return GetType() == obj.GetType();
        }
    }
}