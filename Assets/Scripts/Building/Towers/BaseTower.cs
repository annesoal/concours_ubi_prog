using System;
using System.Collections;
using System.Collections.Generic;
using Building.Towers;
using Enemies;
using Ennemies;
using Grid;
using Grid.Interface;
using Unity.Netcode;
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

    public bool HasFinishedAnimation;

    public BaseTower()
    {
        _timeSinceLastShot = TimeBetweenShots;
    }

    public void Start()
    {
        SetShooter();
    }

    public abstract int AttackDamage { get; set; }

    public abstract int Health { get; set; }
    public abstract int TimeBetweenShots { get; set; }
    public abstract int Range { get; set; }
    public abstract int TotalOfProjectile { get; set; }

    public int  Damage(int damage)
    { 
        return Health -= damage;
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
        TowerManager.Instance.towersInGame.Add(toAdd);
    }

    public override TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Building;
    }

    public bool CanPlay()
    {
        return _timeSinceLastShot++ >= TimeBetweenShots;
    }


    public void UnregisterTower(BaseTower toDelete)
    {
        TowerManager.Instance.towersInGame.Remove(toDelete);
    }
    private IEnumerator ShootAt(Vector3 position)
    {
        RotationAnimation rotationAnimation = new();
        StartCoroutine(rotationAnimation.TurnObjectTo(this.gameObject, position));
        yield return rotationAnimation.HasMoved();
        
        _shooter.FireBetween(shootingPoint.position, position);
        yield return new WaitUntil(_shooter.HasFinished);
        _hasPlayed = true;
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

    public TowerPlayInfo GetPlay()
    {
        TowerPlayInfo towerPlayInfo = new TowerPlayInfo();
        if (!CanPlay())
        {
            //Debug.LogWarning(("Cant play"));
            return towerPlayInfo;
        }

        List<Cell> cellsToShoot = TargetEnemies();
        if (cellsToShoot.Count == 0)
        {
            //Debug.LogWarning("no enemies around");
            return towerPlayInfo;
        }

        towerPlayInfo = FillListToShoot(cellsToShoot);
        return towerPlayInfo;
    }

    private TowerPlayInfo FillListToShoot(List<Cell> cellsToShoot)
    {
        TowerPlayInfo towerPlayInfo = new TowerPlayInfo()
        {
            hasFired = true,
        };
        towerPlayInfo.listEnemiesToShoot = new List<EnemyInfoToShoot>();
        foreach (Cell cell in cellsToShoot)
        {
            EnemyInfoToShoot enemyInfoToShoot = new();
            Enemy enemy = cell.GetEnemy();
            int remainingHP = enemy.Damage(AttackDamage);
            if (remainingHP < 0)
            {
                enemyInfoToShoot.enemy = enemy;
                enemyInfoToShoot.shouldKill = true;
                enemyInfoToShoot.position = enemy.ToGameObject().transform.position;
                enemy.CleanUp();
            }
            else
            {
                enemyInfoToShoot.enemy = enemy;
                enemyInfoToShoot.shouldKill = false;
                enemyInfoToShoot.position = enemy.ToGameObject().transform.position;
            }
            towerPlayInfo.listEnemiesToShoot.Add(enemyInfoToShoot);
        }
        _timeSinceLastShot = 0;
        return towerPlayInfo;
    }


    public IEnumerator PlayAnimation(TowerPlayInfo playInfo)
    {
        HasFinishedAnimation = false;
        if (!playInfo.hasFired)
        {
            HasFinishedAnimation = true;
            yield break;
        }

        var list = playInfo.listEnemiesToShoot;
        foreach (var shotInfo in list)
        {
            Vector3 originToPush = this.gameObject.transform.position;
            originToPush.y = TilingGrid.TopOfCell;
            yield return ShootAt(shotInfo.position);
       
            yield return shotInfo.enemy.PushBackAnimation(originToPush);
            if (shotInfo.shouldKill)
            {
                shotInfo.enemy.Kill();
            }
        }

        HasFinishedAnimation = true;
    }

    public void DestroyThis()
    {
        DestroyClientRpc();
    }

    [ClientRpc]
    private void DestroyClientRpc()
    {
       Destroy(this.gameObject); 
    }

    public void Clean()
    {
        UnregisterTower(this);
        TilingGrid.grid.RemoveObjectFromCurrentCell(this.gameObject);
    }
}