using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleBuildableContentButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private SingleBuildableContentTemplateUI associatedTemplate;
    [SerializeField] private CameraController cameraController;

    private GameObject _buildableObjectPreview;

    public void OnSelect(BaseEventData eventData)
    {
        Vector2Int cameraDestinationInt = associatedTemplate.GetAssociatedCellPosition();
        
        Vector3 cameraDestination = TilingGrid.GridPositionToLocal(cameraDestinationInt, 0f);
        
        cameraController.MoveCameraToPosition(cameraDestination);

        if (HasNotBuildingOnTop(associatedTemplate.GetAssociatedCellITopOfCells()))
        {
            InstantiatePreviewAtPosition(cameraDestination + Vector3.up * TilingGrid.TopOfCell);
        }
    }
    
    private bool HasNotBuildingOnTop(List<ITopOfCell> objectsOnTopOfCell)
    {
        foreach (ITopOfCell objectOnTop in objectsOnTopOfCell)
        {
            if (objectOnTop.GetType() == TypeTopOfCell.Building)
            {
                return false;
            }
        }

        return true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DestroyPreview();
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
}
