using System;
using UnityEngine;

namespace Amulets
{
    
    [CreateAssetMenu(menuName = "AmuletSelector")]
    public class AmuletSelector : ScriptableObject
    {
        public static int PlayerAmuletSelection = 2;
        public AmuletSO AmuletToUse;

        public static AmuletSelector Instance;

        [SerializeField] private AmuletSO[] _amulets;

        private void Awake()
        {
            Instance = this;
        }

        public AmuletSO[] amulets
        {
            get => _amulets;
            private set => _amulets = value;
        }
        public AmuletSO SetAmulet()
        {
            foreach (var amulet in _amulets)
            {
                if (amulet.ID == PlayerAmuletSelection)
                {
                
                    AmuletToUse = amulet;
                    return AmuletToUse;
                }
            }

            throw new Exception("Amulet " + PlayerAmuletSelection + " does not exist!");
        }

 

    }
}
