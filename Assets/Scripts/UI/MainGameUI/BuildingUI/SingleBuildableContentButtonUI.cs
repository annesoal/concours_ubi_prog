using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleBuildableContentButtonUI : MonoBehaviour, ISelectHandler
{
    [SerializeField] private SingleBuildableContentTemplateUI associatedTemplate;
    [SerializeField] private CameraController cameraController;
    
    public void OnSelect(BaseEventData eventData)
    {
        Vector2Int cameraDestinationInt = associatedTemplate.GetAssociatedCellPosition();
        
        Vector3 cameraDestination = TilingGrid.GridPositionToLocal(cameraDestinationInt);
        
        // TODO move camera to destination
        cameraController.MoveCameraToPosition(cameraDestination);
        
        // TODO highlight destination
        Debug.Log("ON SELECT : " + cameraDestinationInt);
    }
}
