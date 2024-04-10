using System.Collections;
using System.Collections.Generic;
using Ennemies;
using Grid;
using Grid.Interface;
using Interfaces;
using UnityEngine;
using Utils;

/**
 * Classe d'exemple d'une tour de base.
 *
 * Cette classe est destinée à être héritée par des tours plus spécifiques.
 * Elle contient tous les comportements communs aux tours.
 */
public abstract class BaseTower : BuildableObject, IDamageable, ICanDamage
{
    private static List<BaseTower> _towersInGame = new();

    public static int baseHealth;
    public static int baseAttack;


    private static bool _hasFinishedTowersTurn;
    public static int baseCost;
    [SerializeField] protected Animator animator;
    [Header("Tower specifics")] [SerializeField]
    protected Transform shootingPoint;

    [SerializeField] protected BuildableObjectVisuals towerVisuals;

    [Header("Projectiles setting")] [SerializeField]
    protected int numberOfProjectilesToShootInTurn;

    [SerializeField] protected EnemyDirection enemyDirection;
    [SerializeField] private float timeToFly;

    /// En radiant
    [SerializeField] private float firingAngle;

    [SerializeField] private int timeBetweenShots = 2;

    [Header("BulletToFire")] [SerializeField]
    protected GameObject _bullet;

    private readonly int _attackDamage = baseAttack;
    private readonly int _cost = baseCost;
    private bool _hasPlayed = true;

    private ShootingUtility _shooter;
    private int _timeSinceLastShot;

    public override int Cost
    {
        get => _cost;
        set => value = _cost;
    }

    public void Start()
    {
        SetShooter();
    }

    public int AttackDamage
    {
        get => _attackDamage;
        set => value = _attackDamage;
    }

    public int Health { get; set; } = baseHealth;

    public void Damage(int damage)
    {
        Health -= damage;
        if (Health < 1)
        {
            Die();
        }
    }

    private void Die()
    {
        TilingGrid.RemoveElement(gameObject, transform.position);
        Destroy(this.gameObject);
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

    public static IEnumerator PlayTowersInGameTurn()
    {
        _hasFinishedTowersTurn = false;
        foreach (var tower in _towersInGame)
            if (tower.CanPlay())
            {
                tower._hasPlayed = false;
                tower.PlayTurn();
                yield return new WaitUntil(tower.HasPlayed);
            }

        _hasFinishedTowersTurn = true;
    }

    public bool CanPlay()
    {
        return _timeSinceLastShot++ >= timeBetweenShots;
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
        _towersInGame = new List<BaseTower>();
    }

    public void PlayTurn()
    {
        StartCoroutine(PlayTurnCoroutine());
        _timeSinceLastShot = 0;
    }

    private IEnumerator PlayTurnCoroutine()
    {
        var cellsWithEnemies = TargetEnemies();

        if (!HasEnemyInRadius(cellsWithEnemies))
        {
            _hasPlayed = true;
            yield break;
        }

        for (var i = 0; i < cellsWithEnemies.Count; i++)
        {
            var cellToFireTo = cellsWithEnemies[i];
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
        var enemyPosition = TilingGrid.CellPositionToLocal(cellWithEnemy);
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
        var enemy = cellWithEnemy.GetEnemy();
        enemy.Damage(AttackDamage);
    }

    private void SetShooter()
    {
        _shooter = gameObject.AddComponent<ShootingUtility>();
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
        XNegative
    }
}