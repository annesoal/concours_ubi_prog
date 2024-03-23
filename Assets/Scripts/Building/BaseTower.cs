using System.Collections;
using System.Collections.Generic;
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

    [field: Header("Buildable Object")]
    [SerializeField] protected BuildableObjectSO buildableObjectSO;

    [Header("Tower specifics")]
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected BuildableObjectVisuals towerVisuals;

    private static List<BaseTower> _towersInGame = new List<BaseTower>();

    protected ShootingUtility shooter; 

    public abstract void Build(Vector2Int positionToBuild);

    public abstract BuildableObjectSO GetBuildableObjectSO();

    public abstract void PlayTurn();

    protected abstract void SetShooter();
        
    public new TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Building;
    }

    public GameObject ToGameObject()
    {
        return gameObject;
    }

    public static void PlayTowersInGameTurn()
    {
        foreach (BaseTower tower in _towersInGame)
        {
            tower.PlayTurn();
        }
    }
    
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

    public abstract void FireOnEnemy();
    
}
