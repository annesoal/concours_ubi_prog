using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TowerSO : ScriptableObject
{
    public GameObject towerPrefab;

    public Sprite towerIcon;

    public string towerName;

    public Transform towerVisuals;

    public string description;

    public List<BuildingMaterialAndQuantityPair> materialAndQuantityPairs;

    /// <summary>
    /// Paire de matériel requis pour la construction de la tour avec la quantité du matériel requis pour la construction.
    /// </summary>
    [Serializable]
    public struct BuildingMaterialAndQuantityPair
    {
        public int quantityOfMaterialRequired;
        public BuildingMaterialSO buildingMaterialSO;
    }
}
