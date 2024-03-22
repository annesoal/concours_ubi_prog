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
        Vector2Int cameraDestinationInt = associatedTemplate.GetAssociatedCellPosition();
        
        Vector3 cameraDestination = TilingGrid.GridPositionToLocal(cameraDestinationInt, 0f);
        
        cameraController.MoveCameraToPosition(cameraDestination);

        if (associatedTemplate.AssociatedCellHasNotBuildingOnTop())
        {
            InstantiatePreviewAtPosition(cameraDestination + Vector3.up * TilingGrid.TopOfCell);
            BasicShowHide.Hide(errorText.gameObject);
        }
        else
        {
            errorText.text = ALREADY_HAS_BUILDING_MESSAGE;
            BasicShowHide.Show(errorText.gameObject);
        }
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

    public void ShowAlreadyHasObjectText()
    {
        errorText.text = ALREADY_HAS_BUILDING_MESSAGE;
        
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
