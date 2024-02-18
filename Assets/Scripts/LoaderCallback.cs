using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool _isFirstUpdate = true;
    void Update()
    {
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            
            Loader.LoaderCallback();
        }
    }
}
