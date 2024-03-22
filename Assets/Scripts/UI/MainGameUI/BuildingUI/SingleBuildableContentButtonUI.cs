using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleBuildableContentButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private SingleBuildableContentTemplateUI associatedTemplate;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TextMeshProUGUI errorText;

    private GameObject _buildableObjectPreview;

    private const string ALREADY_HAS_BUILDING_MESSAGE = "The selected block already has a building on it !";

    private void Start()
    {
        TowerDefenseManager.Instance.OnCurrentStateChanged += TowerDefenseManager_OnCurrentStateChanged;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Vector3 cameraDestination = MoveCameraToBuildingCellPosition();
        
        TrySpawnPreviewAtPosition(cameraDestination);

        if (CantBuildTemplate())
        {
            errorText.text = "You don't have the resources necessary for " +
                             associatedTemplate.GetAssociatedBuildableObjectSO().objectName;
            BasicShowHide.Show(errorText.gameObject);
        }
    }

    /// <returns>The world position at which the camera was moved,
    ///          without the vertical offset that puts objects on top of blocks.</returns>
    private Vector3 MoveCameraToBuildingCellPosition()
    {
        Vector2Int cameraDestinationInt = associatedTemplate.GetAssociatedCellPosition();
        
        Vector3 cameraDestination = TilingGrid.GridPositionToLocal(cameraDestinationInt, 0f);
        
        cameraController.MoveCameraToPosition(cameraDestination);

        return cameraDestination;
    }

    /// <param name="spawnPosition">The spawn position WITHOUT the vertical offset that put objects on top of block.</param>
    private void TrySpawnPreviewAtPosition(Vector3 spawnPosition)
    {
        if (associatedTemplate.AssociatedCellHasNotBuildingOnTop())
        {
            InstantiatePreviewAtPosition(spawnPosition + Vector3.up * TilingGrid.TopOfCell);
            BasicShowHide.Hide(errorText.gameObject);
        }
        else
        {
            ShowErrorText(ALREADY_HAS_BUILDING_MESSAGE);
        }
    }

    private bool CantBuildTemplate()
    {
        return ! CentralizedInventory.Instance.HasResourcesForBuilding(
            associatedTemplate.GetAssociatedBuildableObjectSO()
        );
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        ResetUI();
    }

    private void ResetUI()
    {
        DestroyPreview();
        
        BasicShowHide.Hide(errorText.gameObject);
    }

    private void InstantiatePreviewAtPosition(Vector3 position)
    {
        _buildableObjectPreview = Instantiate(associatedTemplate.GetAssociatedBuildableObjectSO().visuals);
        
        _buildableObjectPreview.GetComponent<BuildableObjectVisuals>().ShowPreview();
        
        _buildableObjectPreview.transform.position = position;
    }
    
    public void DestroyPreview()
    {
        if (_buildableObjectPreview != null)
        { 
            Destroy(_buildableObjectPreview);
            _buildableObjectPreview = null;
        }
    }
    
    public void UpdatePreviewAfterBuilding()
    {
        DestroyPreview();
        
        ShowErrorText(ALREADY_HAS_BUILDING_MESSAGE);
    }

    public void ShowErrorText(string error)
    {
        errorText.text = error;
        
        BasicShowHide.Show(errorText.gameObject);
    }
    
    private void TowerDefenseManager_OnCurrentStateChanged(object sender, TowerDefenseManager.OnCurrentStateChangedEventArgs e)
    {
        if (e.newValue == TowerDefenseManager.State.EnvironmentTurn)
        {
            ResetUI();
        }
    }
}
