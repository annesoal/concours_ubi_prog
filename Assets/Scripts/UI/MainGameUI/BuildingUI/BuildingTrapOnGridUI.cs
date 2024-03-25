using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTrapOnGridUI : MonoBehaviour
{
    [Header("Build")]
    [SerializeField] private Button buildButton;
    
    [Header("Arrows button")]
    [SerializeField] private Button upArrowButton;
    [SerializeField] private Button downArrowButton;
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private Button leftArrowButton;

    [Header("Other")]
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI errorText;

    private void Awake()
    {
        buildButton.onClick.AddListener(() =>
        {
            // TODO
        });
        
        closeButton.onClick.AddListener(() =>
        {
            InputManager.Instance.EnablePlayerInputMap();
            
            BasicShowHide.Hide(gameObject);
            
            CentralizedInventory.Instance.ClearAllMaterialsCostUI();
        });
    }

    private List<Cell> _enemyWalkableCells;
    
    private void Start()
    {
        BasicShowHide.Hide(gameObject);
    }

    public void Show(BuildableObjectSO trapSO)
    {
        Debug.Log(trapSO.objectName);
        
        _enemyWalkableCells = TilingGrid.grid.GetEnemyWalkableCells();
        
        BasicShowHide.Show(gameObject);
    }
}
