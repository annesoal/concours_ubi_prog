using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingMaterialSO : ScriptableObject
{
    public GameObject buildingMaterialPrefab;
    
    public Sprite buildingMaterialicon;

    public string buildingMaterialname;

    public BuildingMaterialType type;
    
    // Make sure to update CentralizedInventory.cs switch case base on building material type
    public enum BuildingMaterialType
    {
        GreyMaterial,
    }
}
