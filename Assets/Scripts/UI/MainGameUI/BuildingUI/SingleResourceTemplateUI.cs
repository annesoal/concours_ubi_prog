using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class SingleResourceTemplateUI : MonoBehaviour
{
    [field: SerializeField] public BuildingMaterialSO ResourceData { get; private set; }

    [SerializeField] private TextMeshProUGUI numberOfResourcesText;
    [SerializeField] private TextMeshProUGUI costText;

    private RectTransform _costTextRectTransform;
    private Vector2 _initialCostTextAnchoredPosition;
    
    private void Awake()
    {
        _costTextRectTransform = costText.GetComponent<RectTransform>();
        
        _initialCostTextAnchoredPosition = _costTextRectTransform.anchoredPosition;
    }

    private void Start()
    {
        BasicShowHide.Hide(costText.gameObject);
    }


    private const string NUMBER_OF_RESOURCES_TEXT = "RESOURCES AVAILABLE : ";
    
    public void SetNumberOfResource(int newValue)
    {
        numberOfResourcesText.text = NUMBER_OF_RESOURCES_TEXT + newValue;
    }

    // At font size 55 for cost and font size 36 for number of resource text
    private const int HORIZONTAL_COST_OFFSET_FACTOR = 25;

    private const string COST_TEXT_BEFORE = "- ";
    
    public void ShowResourceCost(int numberOfResourcesAvailable, int cost)
    {
        SetCostTextPosition(numberOfResourcesAvailable);
        
        costText.text = COST_TEXT_BEFORE + cost;
        
        BasicShowHide.Show(costText.gameObject);
    }
    
    public void ClearMaterialCostUI()
    {
        BasicShowHide.Hide(costText.gameObject);
    }

    private void SetCostTextPosition(int numberOfResourcesAvailable)
    {
        int numberOffsets = ComputeNumberOffsets(numberOfResourcesAvailable);

        int horizontalCostOffset = HORIZONTAL_COST_OFFSET_FACTOR * numberOffsets;

        Debug.Log("Numbers of offsets = " + numberOffsets);
        Debug.Log("initial cost anchoredPosition : " + _initialCostTextAnchoredPosition);
        _costTextRectTransform.anchoredPosition = _initialCostTextAnchoredPosition + Vector2.right * horizontalCostOffset;
        Debug.Log("destination" + _costTextRectTransform.anchoredPosition);
    }

    private const int STARTING_MODULO_COMPUTE_OFFSETS = 10;
    private int ComputeNumberOffsets(int numberOfResourcesAvailable, int modulo = STARTING_MODULO_COMPUTE_OFFSETS)
    {
        if (numberOfResourcesAvailable == 0) { return 0; }

        if (numberOfResourcesAvailable % modulo == 0)
        {
            return 1 + ComputeNumberOffsets(numberOfResourcesAvailable, modulo * 10);
        }
        else
        {
            return 0;
        }
    }
}
