using UnityEngine;

namespace Grid.Blocks
{
    public class BlockBossSpawn : BasicBlock
    {
        [SerializeField] private Transform spawnPosition;

        BlockBossSpawn() : base()
        {
            blockType = BlockType.SpawnBlock;
        }
        
        /**
         * Place le boss sur le bloc.
         */
        public void SetBossOnBlock(Transform boss)
        {
            boss.position = spawnPosition.position;
        }
    }
}