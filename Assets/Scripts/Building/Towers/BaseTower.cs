using System.Collections;
using System.Collections.Generic;
using Building.Towers;
using Ennemies;
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
public abstract class BaseTower : BuildableObject, IDamageable
{
    private static List<BaseTower> _towersInGame = new();

    private static bool _hasFinishedTowersTurn;
    [SerializeField] protected Animator animator;
    [Header("Tower specifics")] [SerializeField]
    protected Transform shootingPoint;

    [SerializeField] protected BuildableObjectVisuals towerVisuals;

    [SerializeField] protected EnemyDirection enemyDirection;
    [SerializeField] private float timeToFly;

    /// En radiant
    [SerializeField] private float firingAngle;

    [Header("BulletToFire")] [SerializeField]
    protected GameObject _bullet;

    private bool _hasPlayed = true;

    private ShootingUtility _shooter;
    private int _timeSinceLastShot;

    private Dictionary<BaseTower, TowerPlayInfo> listOfPlays = new();

    public abstract override int Cost { get; set; }


    public void Start()
    {
        SetShooter();
    }

    public abstract int AttackDamage { get; set; }

    public abstract int Health { get; set; }
    public abstract int TimeBetweenShots { get; set; }
    public abstract int Range { get; set; }
    public abstract int TotalOfProjectile { get; set; }

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
        UnregisterTower(this);
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
        return _timeSinceLastShot++ >= TimeBetweenShots;
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