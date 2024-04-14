using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid.Blocks
{
    public class BasicBlock : MonoBehaviour
    {
        [SerializeField] private List<Type> Types = new ();
        public int blockType;

        protected BasicBlock()
        {

        }

        public Vector3 GetPosition()
        {
            return this.transform.localPosition;
        }

        public int GetBlockType()
        {
            ComputeBlockType();
            
            return blockType;
        }

        private void ComputeBlockType()
        {
            foreach (var type in Types)
            {
                int typeToAdd = BlockType.Translate(type);
                Add(typeToAdd);
            } 
        }

        private void Add(int type)
        {
            if ((blockType & type) == 0)
            {
                blockType = (blockType | type);
            }
        }
    }


}
