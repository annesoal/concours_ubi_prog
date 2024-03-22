using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Interface;
using Unity.Netcode;
using UnityEngine;

public class CentralizedInventory : MonoBehaviour
{
    public static CentralizedInventory Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public NetworkVariable<int> NumberOfResources { get; private set; } = new NetworkVariable<int>(0);

    public void AddResource(ITopOfCell element)
    {
        NumberOfResources.Value++;
    }
}
