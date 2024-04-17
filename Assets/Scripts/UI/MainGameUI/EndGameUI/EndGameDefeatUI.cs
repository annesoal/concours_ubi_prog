using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using Sound;
using UI;
using Unity.Netcode;
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
        TowerDefenseManager.ResetPlayerAmuletSelection();
        if (NetworkManager.Singleton.IsServer)
        {
            StartCoroutine(GoBackToCharacterSelectScene());
        }
    }

    private IEnumerator GoBackToCharacterSelectScene()
    {
        yield return new WaitForSeconds(5);
        
        Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
    }
}
