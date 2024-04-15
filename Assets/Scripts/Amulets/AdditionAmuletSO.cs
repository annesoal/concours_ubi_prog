using System.Collections;
using System.Collections.Generic;
using Amulets;
using UnityEngine;

[CreateAssetMenu(menuName = "AdditionAmuletteSO")]
public class AdditionAmuletSO : ScriptableObject
{
    [Header("Information")]
    public string description;
    public Sprite amuletIcon;

    [Header("Game")]
    public float turnTime = 0f;
    public int startingTurn = 0;
    public int numberOfTurns = 0;
    public float ressourceSpawnRate = 0f;
        
    [Header("Player")] 
    public int playersHealth = 0;
    public int playerEnergy = 0;
    public int startingMoney = 0;

    [Header("Enemis")]
    public int enemyEnergy = 0;

    [Header("Merde")] 
    public int MerdeHeathPoints = 0; 
    public int MerdeMoveRatio = 0;

    [Header("Goofy")]
    public int GoofyHealthPoints = 0; 
    public int GoofyMoveRatio = 0;

    [Header("BigGuy")] 
    public int BigGuyHealthPoints = 0;
    public int BigGuyMoveRatio = 0;
    public int BigGuyDamages = 0;
        
    [Header("Sniper")] 
    public int SniperHealthPoints = 0;
    public int SniperMoveRatio = 0;
    public int SniperDamages = 0;
    public int SniperRange = 0;

    [Header("Trap")] 
    public int TrapCost = 0;
    public int StunDuration = 0;
    public int TrapRange = 0;

    [Header("Obstacles")] 
    public int ObstaclesHealth = 0;

    [Header("Tower")] 
    public int TowerCost = 0;
    public int TowerRange = 0;
    public int TowerHealth = 0;
    public int TowerTimeBetweenAttacks = 0;
    public int numberOfProjectile = 0;
    public int TowerDamage = 0;
        
    [Header("Bomb")] 
    public int BombCost = 0;
    public int BombDamage = 0;
    public int BombRange = 0;
    
}
