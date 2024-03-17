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
    public override void Build(Cell buildableCell)
    {
        GameObject instance = Instantiate(gameObject);
        BasicTower basicTower = instance.GetComponent<BasicTower>();
        
        basicTower.towerVisuals.HidePreview();
        basicTower.transform.position = TilingGrid.GridPositionToLocal(buildableCell.position);
        
        // TODO le reste
        Debug.Log("Build dans basic tower pour cell at position 2int : " + buildableCell.position);
    }

    public override BuildableObjectSO GetBuildableObjectSO()
    {
        return buildableObjectSO;
    }
}
