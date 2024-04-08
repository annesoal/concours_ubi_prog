using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.Netcode;
using UnityEngine;

public class AnimatorConnector : MonoBehaviour
{
    public void Destroy()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("inside destroy");
            GameObject.Destroy(transform.parent.parent.gameObject);
        }
    }
}
