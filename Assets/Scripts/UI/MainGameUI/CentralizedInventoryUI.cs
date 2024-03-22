using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CentralizedInventoryUI : MonoBehaviour
{
    [SerializeField] private List<SingleResourceTemplateUI> resourceTemplates;

    void Start()
    {
        // TODO CHANGE FOR CUSTOM EVENT, WITH THE BUILDABLE OBJECT SO OF WHICH THE VALUE HAS CHANGED
        CentralizedInventory.Instance.NumberOfGreyResources.OnValueChanged += CentralizedInventory_OnNumberOfGreyResourcesChanged;
    }
    
    private void CentralizedInventory_OnNumberOfGreyResourcesChanged(int previousValue, int newValue)
    {
        foreach (SingleResourceTemplateUI template in resourceTemplates)
        {
            if (template.ResourceData.type == BuildingMaterialSO.BuildingMaterialType.GreyMaterial)
            {
                template.SetNumberOfResource(newValue);
                break;
            }
        }
    }

    public void ShowCostForResource(BuildingMaterialSO resourceData, int availableNumber, int cost)
    {
        foreach (SingleResourceTemplateUI template in resourceTemplates)
        {
            if (template.ResourceData == resourceData)
            {
                Debug.Log("Juste before show individual resource cost in CentralizedInventoryUI.cs");
                template.ShowResourceCost(availableNumber, cost);
                break;
            }
        }
    }
    
    public void ClearAllMaterialsCostUI()
    {
        foreach (SingleResourceTemplateUI template in resourceTemplates)
        {
            template.ClearMaterialCostUI();
        }
    }
}
