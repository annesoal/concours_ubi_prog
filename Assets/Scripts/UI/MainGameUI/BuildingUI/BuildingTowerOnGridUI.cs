using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTowerOnGridUI : MonoBehaviour
{
    [SerializeField] private Button closeUIButton;
    
    [Header("Layout")]
    [SerializeField] private int maxNumberOfContentPerLayout;
    [SerializeField] private Transform leftContentLayout;
    [SerializeField] private Transform rightContentLayout;
    
    [Header("Template")]
    [SerializeField] private Transform buildableContentTemplate;
    
    private List<Cell> _buildableCells;
    
    private TowerSO _selectedTowerSo;

    private void Awake()
    {
        closeUIButton.onClick.AddListener(() =>
        {
            // TODO enable back player input
            
            BasicShowHide.Hide(gameObject);
        });
    }

    private void Start()
    {
        SingleTowerSelectUI.OnAnySingleTowerSelectUISelected += SingleTowerSelectUI_OnAnySingleTowerSelectUISelected;
        
        // DEBUG
        // _buildableCells = TilingGrid.grid.GetBuildableCells();
        
        // AddBlocksToContentLayout();
    }

    public void Show(TowerSO selectedTowerSo)
    {
        // TODO disable player input
        
        _selectedTowerSo = selectedTowerSo;
        
        BasicShowHide.Show(gameObject);
    }
    
    private void AddBlocksToContentLayout()
    {
        int i;
        for (i = 0; i < maxNumberOfContentPerLayout; i++)
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
            
        // TODO ADD BUILDABLE CELL AND _selectedTowerSo
        // template.AddBuildableCell(_buildableCells[i], _selectedTowerSo);
    }
    
    private void SingleTowerSelectUI_OnAnySingleTowerSelectUISelected(object sender, SingleTowerSelectUI.TowerData e)
    {
        _selectedTowerSo = e.towerInfos;
    }
}
