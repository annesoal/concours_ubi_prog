using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class EndGameDefeatUI : MonoBehaviour
{
    private void Start()
    {
        TowerDefenseManager.Instance.OnDefeat += TowerDefenseManager_OnDefeat;
        
        BasicShowHide.Hide(gameObject);
    }

    private void TowerDefenseManager_OnDefeat(object sender, EventArgs e)
    {
        BasicShowHide.Show(gameObject);
        StartCoroutine(GoBackToCharacterSelectScene());
    }

    private IEnumerator GoBackToCharacterSelectScene()
    {
        yield return new WaitForSeconds(5);
        
        Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
    }
}
