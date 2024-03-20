using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        CharacterSelectUI.ResetStaticData();
        SingleBuildableObjectSelectUI.ResetStaticData();
        Enemy.ResetSaticData();
        Workshop.ResetStaticData();
    }
}
