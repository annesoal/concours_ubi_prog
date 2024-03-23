using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using Enemies;
using Unity.VisualScripting;

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

    public BasicTower()
    {
        _radius = Radius; 
        _timeToFly = TimeToFly;
        _firingAngle = FiringAngle;
    }

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
}
