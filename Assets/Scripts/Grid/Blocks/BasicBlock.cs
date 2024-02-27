using Unity.Collections;
using UnityEngine;

namespace Grid.Blocks
{
    public enum BlockType
    {
        None = 0,
        Walkable,
        Buildable,
        Movable,
        Destructible,
        SpawnBlock1,
        SpawnBlock2,
        BasicBlock, 
    }
    public class BasicBlock : MonoBehaviour
    {
        [SerializeField] protected BlockType blockType;

        protected BasicBlock()
        {
            blockType = BlockType.BasicBlock;
        }

        // Awake est bien dans editor ou mettre en edit mode
        // Peut creer un lag 
        void Awake()
        {
        
        }


        public Vector3 GetPosition()
        {
            return this.transform.localPosition;
        }

        public BlockType GetBlockType()
        {
            return blockType;
        }
    }
}
