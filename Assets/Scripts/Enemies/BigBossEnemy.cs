using Unity.Netcode;
using UnityEngine;

namespace Enemies
{
    public class BigBossEnemy : NetworkBehaviour
    {
        [SerializeField]private SpawnMalus spawnerMalus = new SpawnMalus();
        [SerializeField] private int ratioMovement = 8;
        
        public void SpawnMalus(int energy)
        {
            if (!IsTimeToMove(energy)) return;
            
            spawnerMalus.SpawnMalusOnGridPlayers();
            

        }


        private bool IsTimeToMove(int energy)
        {
            return energy % ratioMovement == 0;
        }

        
    }
}