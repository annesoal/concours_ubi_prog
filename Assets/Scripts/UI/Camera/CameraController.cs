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
        
        HandleCameraZoom();
    }

    [SerializeField] private float moveSpeed;
    
    private void HandleCameraMovement()
    {
        Vector2 cameraInput = InputManager.Instance.GetCameraMoveInput();

        Vector3 inputDirection = transform.forward * cameraInput.y + transform.right * cameraInput.x;

        transform.position += inputDirection * (moveSpeed * Time.deltaTime);

    }

    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxFov = 80f;
    [SerializeField] private float minFov = 10f;
    
    private void HandleCameraZoom()
    {
        float zoomInput = InputManager.Instance.GetCameraZoomInput();

        float newFov = associatedCamera.m_Lens.FieldOfView - zoomInput * zoomSpeed * Time.deltaTime;

        newFov = Mathf.Clamp(newFov, minFov, maxFov);

        associatedCamera.m_Lens.FieldOfView = newFov;
    }
}
