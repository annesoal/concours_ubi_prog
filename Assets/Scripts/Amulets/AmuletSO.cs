using UnityEngine;
using UnityEngine.Serialization;

namespace Amulets
{
    [CreateAssetMenu(menuName = "AmuletteSO")]
    public class AmuletSO :  ScriptableObject
    {
        [Header("Information")]
        public string amuletName;
        public Sprite amuletIcon;
        public string description = "To Be Changed/Overwritten";
        
        [Header("ID")]
        public int ID = 0;
        
        [Header("Health related issues : ")]
        public int playersHealth = 3;
        public int enemyBaseHealth = 1;
        public int towerBaseHealth = 5;
        public int enemyBaseAttack = 1;
        public int towerBaseAttack = 1;
        
        [Header("Mechanical hazards : ")]
        public float tacticalPauseTime = 20.0f;
        public int numberOfTurns = 10;
        public int playerEnergy = 10;
        public int enemiEnergy = 12;
        public float resourcesSpawnRate = 0.2f;
        public int towerBaseCost = 8;
        public int trapBaseCost = 1;
        public int towerRange = 3; 
    }
}
