using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CentralizedInventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberOfResourcesText;

    private const string NUMBER_OF_RESOURCES_TEXT = "RESOURCES AVAILABLE : ";
    
    void Start()
    {
        CentralizedInventory.Instance.NumberOfResources.OnValueChanged += CentralizedInventory_OnNumberOfResourcesChanged;
    }

    private void CentralizedInventory_OnNumberOfResourcesChanged(int previousValue, int newValue)
    {
        numberOfResourcesText.text = NUMBER_OF_RESOURCES_TEXT + newValue;
    }
}
