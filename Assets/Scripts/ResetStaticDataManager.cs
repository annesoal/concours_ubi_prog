using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using Enemies;
using Grid;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        CharacterSelectUI.ResetStaticData();
        SingleBuildableObjectSelectUI.ResetStaticData();
        Enemy.ResetSaticData();
        Workshop.ResetStaticData();
        TowerDefenseManager.ResetStaticData();
        SingleAmuletTemplateUI.ResetStaticData();
        AmuletSelectionButton.ResetStaticData();
        TilingGrid.ResetReachableCells();
        SpawnMalus.ResetStaticData();
    }
}
