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
public abstract class BaseTower : MonoBehaviour, IBuildable, ITopOfCell
{
    protected int _radius;
    protected float _timeToFly;
    protected float _firingAngle;

    [field: Header("Buildable Object")]
    [SerializeField] protected BuildableObjectSO buildableObjectSO;

    [Header("Tower specifics")]
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected BuildableObjectVisuals towerVisuals;
    
    [Header("Projectiles setting")]
    [SerializeField] protected int numberOfProjectilesToShootInTurn;
    [SerializeField] protected EnemyDirection enemyDirection;

    [Header("BulletToFire")]
    [SerializeField] protected GameObject _bullet;
    
    private static List<BaseTower> _towersInGame = new List<BaseTower>();

    protected ShootingUtility shooter; 
    protected bool _hasPlayed = true;

    public abstract void Build(Vector2Int positionToBuild);

    public abstract BuildableObjectSO GetBuildableObjectSO();

    private static bool _hasFinishedTowersTurn;
    public new TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Building;
    }

    public GameObject ToGameObject()
    {
        return gameObject;
    }

    public void Start()
    {
        SetShooter();
    }

    public static IEnumerator PlayTowersInGameTurn()
    {
        Debug.Log("Inside PlayTowersInGameTurn");
        _hasFinishedTowersTurn = false;
        foreach (BaseTower tower in _towersInGame)
        {
            tower._hasPlayed = false;
            tower.PlayTurn();
            yield return new WaitUntil(tower.HasPlayed);
        }
        _hasFinishedTowersTurn = true;
    }

    /// <returns>List of cells targeted by the tower.</returns>
    protected abstract List<Cell> TargetEnemies();
    
    protected void RegisterTower(BaseTower toAdd)
    {
        _towersInGame.Add(toAdd);
    }

    public void UnregisterTower(BaseTower toDelete)
    {
        _towersInGame.Remove(toDelete);
    }

    public static void ResetStaticData()
    {
        _towersInGame = null;
    }

    private static bool HasEnemyInRadius(List<Cell> cellsWithEnemies)
    {
        return cellsWithEnemies.Count > 0; 
    }

    private void DamageEnemy() 
    {
        Debug.Log("Damage an enemy");
    }

    private bool HasPlayed()
    {
        return _hasPlayed;
    }
    public static bool HasFinishedTowersTurn()
    {
        return _hasFinishedTowersTurn;
    }
    protected List<Enemy> GetEnemiesInRadius()
    {
        Vector2Int position = TilingGrid.LocalToGridPosition(transform.position);
        List<Cell> cells = TilingGrid.grid.GetCellsInRadius(position, _radius);
        List<Enemy> enemies = Enemy.GetEnemiesInCells(cells);

        return enemies;
    }
    public void PlayTurn()
    {
        Debug.Log("Inside PlayTurn");
        StartCoroutine(PlayTurnCoroutine());
    }

    private IEnumerator PlayTurnCoroutine()
    {
        Debug.Log("Tour basique joue son tour (coroutine)");
        List<Cell> cellsWithEnemies = TargetEnemies();

        Debug.Log("before hasEnemyInRadius");
        if (!HasEnemyInRadius(cellsWithEnemies))
        {
            _hasPlayed = true;
            yield break;
        }

        Debug.Log("Passed hasEnemyInRadius");
        for (int i = 0; i < cellsWithEnemies.Count; i++)
        {
            Debug.Log("number of cells " + cellsWithEnemies.Count);
            Cell cellToFireTo = cellsWithEnemies[i];
            StartCoroutine(FireOnCellWithEnemy(cellToFireTo));
            yield return new WaitUntil(HasPlayed); 
        }

    }

    private void SetShooter()
    {
        shooter = gameObject.AddComponent<Utils.ShootingUtility>();
        shooter.TimeToFly = _timeToFly;
        shooter.Angle = _firingAngle;
        shooter.ObjectToFire = _bullet;
    }

    private IEnumerator FireOnCellWithEnemy(Cell cellWithEnemy)
    {
        Debug.Log("FireEnemy Coroutine inside Tower");
        Vector3 towerPosition = transform.position;
        Vector3 enemyPosition = TilingGrid.CellPositionToLocal(cellWithEnemy); 
        shooter.FireBetween(towerPosition, enemyPosition);
        yield return new WaitUntil(shooter.HasFinished);
        DamageEnemy();
        _hasPlayed = true;
    }



    private Enemy GetHighPriorityEnemy(List<Enemy> enemies)
    {
        Enemy highPriorityEnemy = enemies[0];
        for (int i = 1; i < enemies.Count; i++)
        {
            Enemy currentEnemy = enemies[i];
            if (currentEnemy.distanceFromEnd < highPriorityEnemy.distanceFromEnd)
                highPriorityEnemy = currentEnemy;
        }

        return highPriorityEnemy;
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
