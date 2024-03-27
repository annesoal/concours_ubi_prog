using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Enemies;
using Grid;
using Grid.Interface;
using UnityEngine;
using Utils;

/**
 * Classe d'exemple d'une tour de base.
 *
 * Cette classe est destinée à être héritée par des tours plus spécifiques.
 * Elle contient tous les comportements communs aux tours.
 */
public abstract class BaseTower : BuildableObject
{
    [Header("Tower specifics")]
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected BuildableObjectVisuals towerVisuals;
    
    [Header("Projectiles setting")]
    [SerializeField] protected int numberOfProjectilesToShootInTurn;
    [SerializeField] protected EnemyDirection enemyDirection;
    [SerializeField] private float timeToFly;
    /// En radiant
    [SerializeField] private float firingAngle;
    [SerializeField] private int TowerDamage = 1;

    [SerializeField] private int timeBetweenShots = 2; 

    [Header("BulletToFire")]
    [SerializeField] protected GameObject _bullet;
    
    private static List<BaseTower> _towersInGame = new List<BaseTower>();

    private ShootingUtility _shooter; 
    private bool _hasPlayed = true;
    private int timeSinceLastShot = 0;

    public void Start()
    {
        SetShooter();
    }
    
    public override void Build(Vector2Int positionToBuild)
    {
        towerVisuals.HidePreview();
        
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, positionToBuild);
        
        RegisterTower(this);
    }
    
    /// <returns>List of cells targeted by the tower.</returns>
    protected abstract List<Cell> TargetEnemies();
    
    protected void RegisterTower(BaseTower toAdd)
    {
        _towersInGame.Add(toAdd);
    }

    public override TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Building;
    }

    
    private static bool _hasFinishedTowersTurn;
    
    public static IEnumerator PlayTowersInGameTurn()
    {
        _hasFinishedTowersTurn = false;
        foreach (BaseTower tower in _towersInGame)
        {
            if (tower.CanPlay())
            {
                tower._hasPlayed = false;
                tower.PlayTurn();
                yield return new WaitUntil(tower.HasPlayed);
            }
        }
        _hasFinishedTowersTurn = true;
    }

    public bool CanPlay()
    {
      return timeSinceLastShot++ >= timeBetweenShots; 
    }
    
    public static bool HasFinishedTowersTurn()
    {
        return _hasFinishedTowersTurn;
    }

    public void UnregisterTower(BaseTower toDelete)
    {
        _towersInGame.Remove(toDelete);
    }

    public static void ResetStaticData()
    {
        _towersInGame = null;
    }

    public void PlayTurn()
    {
        Debug.Log("Inside PlayTurn");
        StartCoroutine(PlayTurnCoroutine());
        timeSinceLastShot = 0;
    }
    
    private IEnumerator PlayTurnCoroutine()
    {
        Debug.Log("Tour basique joue son tour (coroutine)");
        List<Cell> cellsWithEnemies = TargetEnemies();

        if (!HasEnemyInRadius(cellsWithEnemies))
        {
            _hasPlayed = true;
            yield break;
        }

        for (int i = 0; i < cellsWithEnemies.Count; i++)
        {
            Cell cellToFireTo = cellsWithEnemies[i];
            StartCoroutine(FireOnCellWithEnemy(cellToFireTo));
            yield return new WaitUntil(HasPlayed); 
        }
    }
    
    private static bool HasEnemyInRadius(List<Cell> cellsWithEnemies)
    {
        return cellsWithEnemies.Count > 0; 
    }

    private IEnumerator FireOnCellWithEnemy(Cell cellWithEnemy)
    {
        Vector3 enemyPosition = TilingGrid.CellPositionToLocal(cellWithEnemy); 
        _shooter.FireBetween(shootingPoint.position, enemyPosition);
        yield return new WaitUntil(_shooter.HasFinished);
        DamageEnemy(cellWithEnemy);
        _hasPlayed = true;
    }
    
    private bool HasPlayed()
    {
        return _hasPlayed;
    }
    
    private void DamageEnemy(Cell cellWithEnemy) 
    {
        Enemy enemy = cellWithEnemy.GetEnemy();
        enemy.Damage(TowerDamage);
    }
    
    private void SetShooter()
    {
        _shooter = gameObject.AddComponent<Utils.ShootingUtility>();
        _shooter.TimeToFly = timeToFly;
        _shooter.Angle = firingAngle;
        _shooter.ObjectToFire = _bullet;
    }

    protected enum EnemyDirection
    {
        ZPositive,
        ZNegative,
        YPositive,
        YNegative,
        XPositive,
        XNegative,
    }
}
