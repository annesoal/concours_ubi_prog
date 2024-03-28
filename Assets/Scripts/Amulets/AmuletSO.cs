using UnityEngine;

namespace Amulets
{
    [CreateAssetMenu(menuName = "AmuletteSO")]
    public class AmuletSO :  ScriptableObject
    {
        private static int _id = 0; 
        public int ID = 0;
        public double EnemiHealthMultiplier = 1.0;
        public int PlayersHealth = 3;
        public float TacticalPauseTime = 20.0f;

        private AmuletSO()
        {
            ID = _id++;
        }
    }
}
