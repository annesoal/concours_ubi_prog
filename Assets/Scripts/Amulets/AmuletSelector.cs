using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Amulets
{
    
    [CreateAssetMenu(menuName = "AmuletSelector")]
    public class AmuletSelector : ScriptableObject
    {
        public static AmuletSO PlayerAmuletSelection;
        public AmuletSO AmuletToUse;

        public static AmuletSelector Instance;

        [SerializeField] private AmuletSO[] _amulets;

        [SerializeField] private AmuletSO defaultAmulet;

        private void Awake()
        {
            Instance = this;
        }

        public AmuletSO[] amulets
        {
            get => _amulets;
            private set => _amulets = value;
        }
        public void SetAmulet()
        {
            if (PlayerAmuletSelection == null)
            {
                AmuletToUse = defaultAmulet;
                return;
            }
            
            AmuletToUse = PlayerAmuletSelection;
        }

        public static void ResetPlayerAmuletSelection()
        {
            PlayerAmuletSelection = null;
        }
    }
}
