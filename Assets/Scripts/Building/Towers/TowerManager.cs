using System.Collections;
using System.Collections.Generic;

namespace Building.Towers
{
    public class TowerManager
    {
        public static List<BaseTower> towersInGame = new();
        private static Dictionary<BaseTower, TowerPlayInfo> listPlays = new();
        public static void PlayBackEnd()
        {
            foreach (var tower in towersInGame)
            {
                TowerPlayInfo towerPlayInfo = tower.GetPlay();
                listPlays.Add(tower, towerPlayInfo);

            }
        }

        public static void ResetStaticData()
        {
            towersInGame = new List<BaseTower>();
        }


        public static IEnumerator AnimateTowers()
        {
            List<BaseTower> playingAnimations = new();
            
            foreach (var towerPlayInfo in listPlays)
            {
                towerPlayInfo.Key.PlayAnimation(towerPlayInfo.Value);
                playingAnimations.Add(towerPlayInfo.Key);
            }

            while (playingAnimations.Count > 0)
            {
                foreach (int i = 0; i < 
                {
                    
                }
            }
        }
    }
}