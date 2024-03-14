using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera associatedCamera;
    
    void Update()
    {
        HandleCameraMovement();

        HandleCameraRotation();
        
        HandleCameraZoom();
    }

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    
    private void HandleCameraMovement()
    {
        Vector2 cameraInput = InputManager.Instance.GetCameraMoveInput();

        Vector3 inputDirection = transform.forward * cameraInput.y + transform.right * cameraInput.x;

        transform.position += inputDirection * (moveSpeed * Time.deltaTime);

    }

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 40f;
    
    private void HandleCameraRotation()
    {
        float rotationInput = InputManager.Instance.GetCameraRotationInput();

        transform.Rotate(Vector3.up * (rotationInput * Time.deltaTime * rotationSpeed));
    }

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxFov = 80f;
    [SerializeField] private float minFov = 10f;
    [Header("Vertical View Offset")]
    [SerializeField] private bool verticalViewIsChangingOnZoom = true;
    [SerializeField] private float verticalViewFactor = 25f;
    
    private void HandleCameraZoom()
    {
        float zoomInput = InputManager.Instance.GetCameraZoomInput();

        float newFov = associatedCamera.m_Lens.FieldOfView - zoomInput * zoomSpeed * Time.deltaTime;

        newFov = Mathf.Clamp(newFov, minFov, maxFov);

        associatedCamera.m_Lens.FieldOfView = newFov;

        if (verticalViewIsChangingOnZoom)
        {
            ChangeVerticalView(zoomInput);
        }
    }

    private void ChangeVerticalView(float zoomInput)
    {
        CinemachineTransposer associatedCameraTransposer = 
            associatedCamera.GetCinemachineComponent<CinemachineTransposer>();

        Vector3 newFollowOffset = associatedCameraTransposer.m_FollowOffset - Vector3.up * zoomInput;

        associatedCameraTransposer.m_FollowOffset = 
            Vector3.Lerp(associatedCameraTransposer.m_FollowOffset, newFollowOffset, Time.deltaTime);
    }
}
