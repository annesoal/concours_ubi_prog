using System;
using System.Collections;
using System.Collections.Generic;
using Building.Towers;
using Enemies;
using Ennemies;
using Grid;
using Grid.Interface;
using Sound;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

/**
 * Classe d'exemple d'une tour de base.
 *
 * Cette classe est destinée à être héritée par des tours plus spécifiques.
 * Elle contient tous les comportements communs aux tours.
 */
public abstract class BaseTower : BuildableObject, IDamageable
{
    [SerializeField] protected AudioClip damageAudioClip;
    [FormerlySerializedAs("buildAudioSound")] [SerializeField] protected AudioClip buildAudioClip;
    [SerializeField] protected Animator animator;
    [Header("Tower specifics")] [SerializeField]
    protected Transform shootingPoint;

    [SerializeField] protected BuildableObjectVisuals towerVisuals;

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

    public override void Build(Vector2Int positionToBuild)
    {
        towerVisuals.HidePreview();
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, positionToBuild);
        playSoundClientRpc();
        RegisterTower(this);
    }

    [ClientRpc]
    public void playSoundClientRpc()
    {
        SoundFXManager.instance.PlaySoundFXCLip(buildAudioClip, transform, 1f);
    }
    
    
    /// <returns>List of cells targeted by the tower.</returns>
    protected abstract List<EnemyInfoToShoot> TargetEnemies();

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
        
        // TODO : modifier le time tofly en fonction de la vitesse 
        _shooter.FireBetween(shootingPoint.position, position);
        yield return new WaitUntil(_shooter.HasFinished);
        _hasPlayed = true;
        SoundFXManager.instance.PlaySoundFXCLip(damageAudioClip, transform,1f);

    }


    private void SetShooter()
    {
        _shooter = gameObject.AddComponent<ShootingUtility>();
        _shooter.TimeToFly = timeToFly;
        _shooter.Angle = firingAngle;
        _shooter.ObjectToFire = _bullet;
    }

    public TowerPlayInfo GetPlay()
    {
        TowerPlayInfo towerPlayInfo = new TowerPlayInfo();
        if (!CanPlay())
        {
            //Debug.LogWarning(("Cant play"));
            return towerPlayInfo;
        }
        
        List<EnemyInfoToShoot> infos = TargetEnemies();
        if (infos.Count == 0)
        {
            //Debug.LogWarning("no enemies around");
            return towerPlayInfo;
        }

        towerPlayInfo.listEnemiesToShoot = infos;
        towerPlayInfo.hasFired = true;
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