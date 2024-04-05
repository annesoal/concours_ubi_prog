using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    
    [SerializeField] private CinemachineVirtualCamera associatedCamera;

    private void Awake()
    {
        Instance = this;

        _followOffset = associatedCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }

    public void MoveCameraToPosition(Vector3 destination)
    {
        transform.position = destination;
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
    [SerializeField] private float maxFollowOffset = 50f;
    [SerializeField] private float minFollowOffset = 10f;
    
    private Vector3 _followOffset;
    
    private void HandleCameraZoom()
    {
        Vector3 zoomDirection = _followOffset.normalized;
        
        float zoomInput = InputManager.Instance.GetCameraZoomInput();

        _followOffset -= zoomDirection * (zoomInput * zoomSpeed * Time.deltaTime);

        if (_followOffset.magnitude < minFollowOffset)
        {
            _followOffset = zoomDirection * minFollowOffset;
        }

        if (_followOffset.magnitude > maxFollowOffset)
        {
            _followOffset = zoomDirection * maxFollowOffset;
        }

        associatedCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = _followOffset;
    }
}
