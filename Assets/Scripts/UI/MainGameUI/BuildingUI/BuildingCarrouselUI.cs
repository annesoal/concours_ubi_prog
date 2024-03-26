using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class BuildingCarrouselUI : MonoBehaviour
{
    [SerializeField] private Image centerImage;
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;

    [SerializeField] private TextMeshProUGUI selectedBuildingText;
    
    private LinkedList<BuildableObjectSO> _buildableObjectsSO;
    private LinkedListNode<BuildableObjectSO> _selectedBuilding;

    private GameObject _preview;

    public event EventHandler<OnBuildingSelectedEventArgs> OnBuildingSelected;
    public class OnBuildingSelectedEventArgs : EventArgs
    {
        public BuildableObjectSO SelectedBuildableObjectSO;
    }
    
    private void Awake()
    {
        _buildableObjectsSO = new LinkedList<BuildableObjectSO>();
    }

    private void Start()
    {
        InitializeBuildableObjectsList();

        _selectedBuilding = _buildableObjectsSO.First;
        
        BasicShowHide.Hide(gameObject);
    }

    public void Show()
    {
        SetCarrouselImages();
        
        ShowPreview();
        
        ShowDescription();
        
        ShowMaterialCost(_selectedBuilding.Value);
        
        BasicShowHide.Show(gameObject);
    }

    public void HideForNextBuildStep()
    {
        HidePreview();
        
        HideDescription();
        
        BasicShowHide.Hide(gameObject);
    }
    
    public void Hide()
    {
        HidePreview();
        
        HideDescription();
        
        ClearOldMaterialCostUI();
        
        BasicShowHide.Hide(gameObject);
    }

    private void SetCarrouselImages()
    {
        if (_buildableObjectsSO.Count == 1)
        {
            centerImage.sprite = _selectedBuilding.Value.icon;
            return;
        }

        if (_selectedBuilding.Previous == null)
        {
            leftImage.sprite = _selectedBuilding.Next.Value.icon;
            rightImage.sprite = _selectedBuilding.Next.Value.icon;

            centerImage.sprite = _selectedBuilding.Value.icon;
            return;
        }

        if (_selectedBuilding.Next == null)
        {
            leftImage.sprite = _selectedBuilding.Previous.Value.icon;
            rightImage.sprite = _selectedBuilding.Previous.Value.icon;

            centerImage.sprite = _selectedBuilding.Value.icon;
            return;
        }
        
        leftImage.sprite = _selectedBuilding.Previous.Value.icon;
        rightImage.sprite = _selectedBuilding.Next.Value.icon;

        centerImage.sprite = _selectedBuilding.Value.icon;
    }
    
    private void ShowMaterialCost(BuildableObjectSO selectedBuildingValue)
    {
        CentralizedInventory.Instance.ShowCostForBuildableObject(selectedBuildingValue);
    }
    
    private void ClearOldMaterialCostUI()
    {
        CentralizedInventory.Instance.ClearAllMaterialsCostUI();
    }

    private void ShowDescription()
    {
        selectedBuildingText.text = _selectedBuilding.Value.description;
        
        BasicShowHide.Show(selectedBuildingText.gameObject);
    }

    private void HideDescription()
    {
        BasicShowHide.Hide(selectedBuildingText.gameObject);
    }
    
    private void ShowPreview()
    {
        HidePreview();

        _preview = Instantiate(_selectedBuilding.Value.visuals);
        
        MovePreviewInFrontOfCamera();
    }

    private const float PREVIEW_FRONT_DISTANCE = 10f;
    private const float PREVIEW_RIGHT_DISTANCE = 9f;
    private const float PREVIEW_ROTATION_SPEED = 6f;
    
    private void MovePreviewInFrontOfCamera()
    {
        GameObject toFollow = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject;

        _preview.AddComponent<RotateObject>().SetRotationParameter(PREVIEW_ROTATION_SPEED);
        
        _preview.AddComponent<FollowTransform>().SetFollowParameters(
            toFollow,
            new Vector3(PREVIEW_RIGHT_DISTANCE, 0, PREVIEW_FRONT_DISTANCE), 
            FollowTransform.DirectionOfFollow.FrontRight
        );
            
        _preview.GetComponent<BuildableObjectVisuals>().ShowPreview();
    }

    private void HidePreview()
    {
        if (_preview != null)
        {
            Destroy(_preview);
            _preview = null;
        }
    }
    
    private void InitializeBuildableObjectsList()
    {
        foreach (BuildableObjectSO buildableObjectSo in SynchronizeBuilding.Instance.GetAllBuildableObjectSo().list)
        {
            _buildableObjectsSO.AddLast(buildableObjectSo);
        }
    }
}
