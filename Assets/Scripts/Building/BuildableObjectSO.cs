using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildableObjectSO : ScriptableObject
{
    /// <summary>
    /// Doit implémenter l'interface IBuildable (l'interface indiquant que l'objet peut être construit.)
    /// </summary>
    public GameObject prefab;

    public Sprite icon;

    public string objectName;

    public GameObject visuals;

    public string description;

    public TypeOfBuildableObject type;

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

    public enum TypeOfBuildableObject
    {
        Tower,
        Trap,
    }
}
