using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class SM_PetiteMerde : MonoBehaviour
{
    public void Destroy()
    {
        Debug.Log("inside destroy");
        GameObject.Destroy(transform.parent.parent.gameObject);
    }
}
