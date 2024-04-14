using UnityEngine;

namespace Building.Traps
{
    public class TrapManager : MonoBehaviour
    {
        public static TrapManager Instance;

        
        public void Awake()
        {
            Instance = this;
        }
        
        public void PlayBackEnd()
        {
            
        }
    }
}