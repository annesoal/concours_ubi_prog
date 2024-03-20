using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

/**
 * Classe d'exemple d'une tour spécifique.
 *
 * Une tour spécifique hérite de la classe BaseTower.
 * La classe implémente les fonctions spécifiques à la tour.
 */
public class BasicTower : BaseTower
{ 
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
}
