using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using Enemies;

/**
 * Classe d'exemple d'une tour spécifique.
 *
 * Une tour spécifique hérite de la classe BaseTower.
 * La classe implémente les fonctions spécifiques à la tour.
 */
public class BasicTower : BaseTower
{ 
    const float TimeToFly = 1.0f; 
    const int Radius = 2;
    const float FiringAngle = 0.4f; // En Radian !
    [SerializeField] private GameObject _Bullet;
    public override void Build(Vector2Int positionToBuild)
    {
        towerVisuals.HidePreview();
        
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, positionToBuild);
        
        RegisterTower(this);
    }

    public override BuildableObjectSO GetBuildableObjectSO()
    {
        return buildableObjectSO;
    }

    public override void PlayTurn()
    {
        Debug.Log("Tour basique joue son tour");
    }

    protected override void SetShooter()
    {
        shooter = new Utils.ShootingUtility();
        shooter.TimeToFly = TimeToFly;
        shooter.ObjectToFire = _Bullet;
        shooter.Angle = FiringAngle;
    }

    public override void FireOnEnemy()
    {
       List<Enemy> enemies = GetEnemiesInRadius(); 
       if (!HasEnemyInRadius(enemies)) return;

       Enemy enemyToFireTo = GetHighPriorityEnemy(enemies);
       
    }

    private List<Enemy> GetEnemiesInRadius()
    {
        Vector2Int position = TilingGrid.LocalToGridPosition(transform.position);
        List<Cell> cells = TilingGrid.grid.GetCellsInRadius(position, Radius);

        return new List<Enemy>();
    }
    private Enemy GetHighPriorityEnemy(List<Enemy> enemies)
    {
        Enemy highPriorityEnemy = enemies[0]; 
        for(int i = 1; i < enemies.Count; i++)
        {
            Enemy currentEnemy = enemies[i];
            if (currentEnemy.distanceFromEnd < highPriorityEnemy.distanceFromEnd)
                highPriorityEnemy = currentEnemy;            
        }
        return highPriorityEnemy;
    }
}
