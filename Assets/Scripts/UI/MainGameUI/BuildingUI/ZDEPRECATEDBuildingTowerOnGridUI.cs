using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UI;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DEPRECATED
/// </summary>
public class ZDEPRECATEDBuildingTowerOnGridUI : MonoBehaviour
{
    [SerializeField] private Button closeUIButton;
    
    [Header("Layout")]
    [SerializeField] private int maxNumberOfContentPerLayout;
    [SerializeField] private Transform leftContentLayout;
    [SerializeField] private Transform rightContentLayout;
    
    [Header("Template")]
    [SerializeField] private Transform buildableContentTemplate;
    
    private List<Cell> _buildableCells;
    
    private BuildableObjectSO _selectedBuildableObject;

    private void Awake()
    {
        closeUIButton.onClick.AddListener(() =>
        {
            InputManager.Instance.EnablePlayerInputMap();
            
            BasicShowHide.Hide(gameObject);
            
            ClearLayouts();
            
            CentralizedInventory.Instance.ClearAllMaterialsCostUI();
        });
    }

    private void Start()
    {
        _buildableCells = TilingGrid.grid.GetBuildableCells();
        
        AddBlocksToContentLayout();
        
        gameObject.SetActive(false);
    }

    public void Show(BuildableObjectSO buildableObjectSO)
    {
        ClearLayouts();
        
        _buildableCells = TilingGrid.grid.GetBuildableCells();
        
        InputManager.Instance.DisablePlayerInputMap();

        _selectedBuildableObject = buildableObjectSO;
        
        AddBlocksToContentLayout();
        
        BasicShowHide.Show(gameObject);
        
        closeUIButton.Select();
    }
    
    private void AddBlocksToContentLayout()
    {
        int i;
        for (i = 0; i < _buildableCells.Count && i < maxNumberOfContentPerLayout; i++)
        {
            InstantiateTemplate(_buildableCells[i], leftContentLayout);
        }

        if (LeftLayoutIsFull(i))
        {
            for (int k = i; k < _buildableCells.Count; k++)
            {
                InstantiateTemplate(_buildableCells[k], rightContentLayout);
            }
        }
    }

    private bool LeftLayoutIsFull(int addedObjects)
    {
        return addedObjects < _buildableCells.Count;
    }

    private void InstantiateTemplate(Cell buildableCell, Transform parentLayout)
    {
        Transform template = Instantiate(buildableContentTemplate, parentLayout);
        
        template.gameObject.SetActive(true);
        
        SingleBuildableContentTemplateUI singleTemplate = template.GetComponent<SingleBuildableContentTemplateUI>();
            
        singleTemplate.SetTemplateInfos(buildableCell, _selectedBuildableObject);
    }

    private void ClearLayouts()
    {
        ClearLayout(leftContentLayout);
        ClearLayout(rightContentLayout);
    }

    private void ClearLayout(Transform layoutToClear)
    {
        foreach (Transform child in layoutToClear)
        {
            if (child != buildableContentTemplate)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
