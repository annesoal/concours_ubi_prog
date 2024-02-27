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

        // Awake est bien dans editor ou mettre en edit mode
        // Peut creer un lag 
        void Awake()
        {
            Debug.Log(blockType); 
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
