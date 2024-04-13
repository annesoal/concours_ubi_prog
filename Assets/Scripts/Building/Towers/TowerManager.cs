using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building.Towers
{
    public class TowerManager : MonoBehaviour
    {

        public static TowerManager Instance;
        public bool HasFinishedAnimations = false;
        public  List<BaseTower> towersInGame = new();
        private  Dictionary<BaseTower, TowerPlayInfo> listPlays = new();

        public void Awake()
        {
            Instance = this;
        }

        public  void PlayBackEnd()
        {
            foreach (var tower in towersInGame)
            {
                TowerPlayInfo towerPlayInfo = tower.GetPlay();
                listPlays.Add(tower, towerPlayInfo);
            }
        }

        public void ResetStates()
        {
            HasFinishedAnimations = false;
            foreach(var towerInGame in towersInGame)
            {
                towerInGame.HasFinishedAnimation = false;
            }
        }

        public  IEnumerator AnimateTowers()
        {
            HasFinishedAnimations = false;
            List<BaseTower> playingAnimations = new();
            
            foreach (var towerPlayInfo in listPlays)
            {
                StartCoroutine(towerPlayInfo.Key.PlayAnimation(towerPlayInfo.Value));
                playingAnimations.Add(towerPlayInfo.Key);
            }

            while (playingAnimations.Count > 0)
            {
                for (int i = 0; i < playingAnimations.Count; i ++)
                {
                    var tower = playingAnimations[i];
                    if (tower.HasFinishedAnimation)
                        playingAnimations.RemoveAt(i);
                }

                yield return null;
            }

            HasFinishedAnimations = true;
        }
    }
}