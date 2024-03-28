using System;
using UnityEngine;

namespace Amulets
{
    public class AmuletSelector : MonoBehaviour
    {
        public static int PlayerAmuletSelection = 0;
        public AmuletSO AmuletToUse;

        [SerializeField] private AmuletSO[] Amulets;

        public AmuletSO SetAmulet()
        {
            foreach (var amulet in Amulets)
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
