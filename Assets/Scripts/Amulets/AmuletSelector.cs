using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Amulets
{
    public class AmuletSelector : MonoBehaviour
    {
        [SerializeField] private AdditionAmuletSO defaultAmulet;
        
        public static AdditionAmuletSO PlayerAmuletSelection;

        private void Awake()
        {
            if (PlayerAmuletSelection == null)
            {
                PlayerAmuletSelection = defaultAmulet;
            }
        }

        public static void ResetPlayerAmuletSelection()
        {
            PlayerAmuletSelection = null;
        }
    }
}
