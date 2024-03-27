using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    /// degrés
    private float _rotationSpeed;

    private bool _isSetUp = false;
    
    public void SetRotationParameter(float rotationSpeed)
    {
        _rotationSpeed = rotationSpeed;
        
        _isSetUp = true;
    }
    
    private void Update()
    {
        if (_isSetUp)
        {
            transform.Rotate(transform.up, _rotationSpeed * Time.deltaTime);
        }
    }
}
