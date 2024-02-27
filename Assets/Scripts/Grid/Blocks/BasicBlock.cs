using Unity.Collections;
using UnityEngine;

namespace Grid.Blocks
{
    

    public class BasicBlock : MonoBehaviour
    {
        [SerializeField] protected int blockType;

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

        public int GetBlockType()
        {
            return blockType;
        }
    }


}
