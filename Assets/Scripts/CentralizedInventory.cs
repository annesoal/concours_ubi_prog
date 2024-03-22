using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Interface;
using Unity.Netcode;
using UnityEngine;

public class CentralizedInventory : NetworkBehaviour
{
    public static CentralizedInventory Instance { get; private set; }

    [SerializeField] private CentralizedInventoryUI correspondingUI;

    private void Awake()
    {
        Instance = this;
    }

    public NetworkVariable<int> NumberOfGreyResources { get; private set; } = new NetworkVariable<int>(0);

    public void AddResource(ITopOfCell element)
    {
        NumberOfGreyResources.Value++;
    }

    public void DecreaseResourceForBuilding(BuildableObjectSO builtObjectSO)
    {
        foreach (BuildableObjectSO.BuildingMaterialAndQuantityPair pair in builtObjectSO.materialAndQuantityPairs)
        {
            DecreaseIndividualResource(pair.buildingMaterialSO, pair.quantityOfMaterialRequired);
        }
    }

    public void ShowCostForResource(BuildingMaterialSO resourceData, int cost)
    {
        try
        {
            correspondingUI.ShowCostForResource(resourceData, GetNumberAvailableResource(resourceData), cost);
        }
        catch (NoMatchingBuildingMaterialSOException e)
        {
            Debug.LogError(e);
        }
    }
    
    public bool HasResourcesForBuilding(BuildableObjectSO buildableObjectSo)
    {
        foreach (BuildableObjectSO.BuildingMaterialAndQuantityPair pair in buildableObjectSo.materialAndQuantityPairs)
        {
            int numberRequired = GetNumberAvailableResource(pair.buildingMaterialSO);
            if (numberRequired < pair.quantityOfMaterialRequired)
            {
                return false;
            }
        }

        return true;
    }

    private void DecreaseIndividualResource(BuildingMaterialSO resourceData, int cost)
    {
        switch (resourceData.type)
        {
            case BuildingMaterialSO.BuildingMaterialType.GreyMaterial:
                NumberOfGreyResources.Value = NumberOfGreyResources.Value - cost;
                break;
        }
    }

    /// Throws NoMatchingBuildingMaterialSOException when the BuildingMaterialSO specified does not exist in inventory.
    private int GetNumberAvailableResource(BuildingMaterialSO resourceData)
    {
        switch (resourceData.type)
        {
            case BuildingMaterialSO.BuildingMaterialType.GreyMaterial:
                return NumberOfGreyResources.Value;
        }

        throw new NoMatchingBuildingMaterialSOException("No match is found, maybe a new building material was" +
                                                        "not coded in the inventory yet !");
    }

    public void ClearAllMaterialsCostUI()
    {
        correspondingUI.ClearAllMaterialsCostUI();
    }
}
