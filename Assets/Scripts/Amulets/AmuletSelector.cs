using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Amulets
{
    
    //[CreateAssetMenu(menuName = "AmuletSelector")]
    public class AmuletSelector : ScriptableObject
    {
        public static AdditionAmuletSO PlayerAmuletSelection;

        [SerializeField] private AdditionAmuletSO defaultAmulet;

        public static void ResetPlayerAmuletSelection()
        {
            PlayerAmuletSelection = null;
        }
    }
}
