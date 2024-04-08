using UnityEngine;

namespace Grid.Blocks
{
    public class BlockPlayerSpawn : BasicBlock
    {
        [SerializeField] private Transform spawnPosition;

        BlockPlayerSpawn() : base()
        {
            blockType = BlockType.SpawnBlock;
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