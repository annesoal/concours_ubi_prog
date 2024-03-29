using UnityEngine;

namespace Amulets
{
    [CreateAssetMenu(menuName = "AmuletteSO")]
    public class AmuletSO :  ScriptableObject
    {
        private static int _id = 0; 
        public int ID = 0;
        [Header("Health related issues : ")]
        public double EnemiHealthMultiplier = 1.0;
        public int PlayersHealth = 3;
        public int EnemiHealth = 1;
        public int TowerHealth = 5;
        [Header("Mechanical hazards : ")]
        public float TacticalPauseTime = 20.0f;
        public int numberOfTurns = 20;
        public int playerEnergy = 10;
        public float ResourcesSpawnRate = 0.2f;
        public int baseTowerCost = 3;
        [Header("Description/Assets : ")]
        public string description = "To Be Changed/Overwritten";
        private AmuletSO()
        {
            ID = _id++;
        }
    }
}
