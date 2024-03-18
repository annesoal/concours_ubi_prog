using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

/**
 * Classe d'exemple d'une tour de base.
 *
 * Cette classe est destinée à être héritée par des tours plus spécifiques.
 * Elle contient tous les comportements communs aux tours.
 */
public abstract class BaseTower : MonoBehaviour, IBuildable
{

    [field: Header("Buildable Object")]
    [SerializeField] protected BuildableObjectSO buildableObjectSO;

    [Header("Tower specifics")]
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected BuildableObjectVisuals towerVisuals;

    public abstract void Build(Vector3 positionToBuild);

    public abstract BuildableObjectSO GetBuildableObjectSO();
}
