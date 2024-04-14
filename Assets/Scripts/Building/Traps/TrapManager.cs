using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Building.Traps
{
    public class TrapManager : MonoBehaviour
    {
        public static TrapManager Instance;
        public List<BaseTrap> trapsInGame = new();
        public bool HasFinishedAnimations = false;

        
        public void Awake()
        {
            Instance = this;
        }
       
        private Dictionary<BaseTrap, TrapPlayInfo> listPlays = new();

        public void PlayBackEnd()
        {
            var copy = new List<BaseTrap>(trapsInGame);
            while (copy.Count > 0)
            {
                BaseTrap trap = copy[0]; 
                copy.RemoveAt(0);
                TrapPlayInfo trapPlayInfo = trap.GetPlay();
                listPlays.Add(trap, trapPlayInfo);
        
            }
        }
        public  IEnumerator AnimateTraps()
        {
            HasFinishedAnimations = false;
            List<BaseTrap> playingAnimations = new();
            
            foreach (var trapPlayInfo in listPlays)
            {
                StartCoroutine(trapPlayInfo.Key.PlayAnimation(trapPlayInfo.Value));
                playingAnimations.Add(trapPlayInfo.Key);
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

        public void ResetAnimations()
        {
            this.listPlays = new();
            foreach (var trap in trapsInGame)
            {
                trap.HasFinishedAnimation = false;
            }

            this.HasFinishedAnimations = false;
        }
      }
}