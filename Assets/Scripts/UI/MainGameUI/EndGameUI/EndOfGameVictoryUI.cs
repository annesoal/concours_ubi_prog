using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class EndOfGameVictoryUI : MonoBehaviour
{
    private void Start()
    {
        TowerDefenseManager.Instance.OnVictory += TowerDefenseManager_OnVictory;
        BasicShowHide.Hide(gameObject);
    }

    private void TowerDefenseManager_OnVictory(object sender, EventArgs e)
    {
        BasicShowHide.Show(gameObject);
    }
}
