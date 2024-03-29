using System;
using UnityEngine;

namespace Amulets
{
    public class AmuletSelector : MonoBehaviour
    {
        public static int PlayerAmuletSelection = 0;
        public AmuletSO AmuletToUse;

        public static AmuletSelector Instance; 
        [SerializeField] private AmuletSO[] _amulets;

        public AmuletSO[] amulets
        {
            get => _amulets;
            private set => _amulets = value;
        }

        private void Awake()
        {
            Instance = this; 
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
