using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera associatedCamera;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoomSpeed;
    
    void Update()
    {
        HandleCameraMovement();
        
        HandleCameraZoom();
    }

    private void HandleCameraMovement()
    {
        Vector2 cameraInput = InputManager.Instance.GetCameraMoveInput();

        Vector3 inputDirection = transform.forward * cameraInput.y + transform.right * cameraInput.x;

        transform.position += inputDirection * (moveSpeed * Time.deltaTime);

    }

    private void HandleCameraZoom()
    {
        float zoomInput = InputManager.Instance.GetCameraZoomInput();

        associatedCamera.m_Lens.FieldOfView -= zoomInput * zoomSpeed * Time.deltaTime;
    }
}
