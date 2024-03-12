using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        CharacterSelectUI.ResetStaticData();
        SingleTowerSelectUI.ResetStaticData();
    }
}
