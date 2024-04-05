using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SynchronizeITopOfCell : NetworkBehaviour
{
    public static SynchronizeITopOfCell Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SynchronizeAddingElement()
    {
        
    }
    
    public void SynchronizeRemovingElement()
    {
        
    }
}
