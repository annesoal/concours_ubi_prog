using UnityEngine;

namespace Grid.Blocks
{
    public class BlockPlayerSpawn : BasicBlock
    {
        [SerializeField] private Transform spawnPosition;
            
        void Awake()
        {
            blockType = BlockType.SpawnBlock1 | BlockType.Walkable;
        }

        /**
         * Place le joueur sur le bloc.
         */
        public void SetPlayerOnBlock(Transform player)
        {
            player.position = spawnPosition.position;
        }
        
    }
}