using System.Collections;
using System.Collections.Generic;
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
            foreach (var trap in trapsInGame)
            {
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
      }
}