using UnityEngine;
using UnityEngine.Serialization;

namespace Amulets
{
    [CreateAssetMenu(menuName = "AmuletteSO")]
    public class AmuletSO :  ScriptableObject
    {
        [Header("Nom")]
        public string ID;
        
        [Header("Game")]
        public float turnTime = 20.0f;
        public int startingTurn = 0;
        public int numberOfTurns = 10;
        public float ressourceSpawnRate = 0.2f;
        
        [Header("Player")] 
        public int playersHealth = 3;
        public int playerEnergy = 10;
        public int startingMoney = 0;

        [Header("Enemis")]
        public int enemyEnergy = 12;

        [Header("Merde")] 
        public int MerdeHeathPoints = 1; 
        public int MerdeMoveRatio = 1;

        [Header("Goofy")]
        public int GoofyHealthPoints = 1; 
        public int GoofyMoveRatio = 1;

        [Header("BigGuy")] 
        public int BigGuyHealthPoints = 1;
        public int BigGuyMoveRatio = 1;
        public int BigGuyDamages = 1;
        
        [Header("Sniper")] 
        public int SniperHealthPoints = 1;
        public int SniperMoveRatio = 1;
        public int SniperDamages = 1;
        public int SniperRange = 1;

        [Header("Trap")] 
        public int TrapCost = 2;
        public int StunDuration = 1;

        [Header("Obstacles")] 
        public int ObstaclesHealth = 1;

        [Header("Tower")] 
        public int TowerCost = 4;
        public int TowerRange = 3;
        public int TowerHealth = 1;
        public int TowerTimeBetweenAttacks = 2;
        public int numberOfProjectile = 1;
        public int TowerDamage = 1;
        
        [Header("Bomb")] 
        [Header("NonUtilisable")]
        public int BombCost = 2;
        public int BombDamage = 1;
    }
}
