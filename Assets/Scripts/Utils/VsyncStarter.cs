using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VsyncStarter : MonoBehaviour
{
    private void Start()
    {
        QualitySettings.vSyncCount = 1;
    }
}
