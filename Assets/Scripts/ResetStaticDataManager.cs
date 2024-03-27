using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        CharacterSelectUI.ResetStaticData();
        SingleBuildableObjectSelectUI.ResetStaticData();
        Enemy.ResetSaticData();
        Workshop.ResetStaticData();
        BaseTower.ResetStaticData();
        TowerDefenseManager.ResetStaticData();
    }
}
