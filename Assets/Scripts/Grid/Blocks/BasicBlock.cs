using Unity.Collections;
using UnityEngine;

namespace Grid.Blocks
{
    

    public class BasicBlock : MonoBehaviour
    {
        protected int blockType;
        protected BasicBlock()
        {
            blockType = BlockType.BasicBlock;
        }

        public Vector3 GetPosition()
        {
            return this.transform.localPosition;
        }

        public int GetBlockType()
        {
            return blockType;
        }
    }


}
