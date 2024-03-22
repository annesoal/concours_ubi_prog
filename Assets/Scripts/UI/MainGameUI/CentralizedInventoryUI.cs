using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CentralizedInventoryUI : MonoBehaviour
{
    [SerializeField] private List<SingleResourceTemplateUI> resourceTemplates;

    void Start()
    {
        CentralizedInventory.Instance.OnNumberResourceChanged += CentralizedInventory_OnNumberResourceChanged;
    }

    public void ShowCostForResource(BuildingMaterialSO resourceData, int availableNumber, int cost)
    {
        foreach (SingleResourceTemplateUI template in resourceTemplates)
        {
            if (template.ResourceData == resourceData)
            {
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
    
    private void CentralizedInventory_OnNumberResourceChanged(object sender, CentralizedInventory.OnNumberResourceChangedEventArgs e)
    {
        foreach (SingleResourceTemplateUI template in resourceTemplates)
        {
            if (template.ResourceData.type == e.ResourceChanged.type)
            {
                template.SetNumberOfResource(e.NewValue);
                break;
            }
        }
    }

}
