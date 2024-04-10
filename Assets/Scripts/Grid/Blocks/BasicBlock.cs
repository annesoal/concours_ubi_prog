using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid.Blocks
{
    public class BasicBlock : MonoBehaviour
    {
        [SerializeField] private List<Type> Types = new ();
        public int blockType;

        void Awake()
        {
            foreach (var type in Types)
            {
                int typeToAdd = BlockType.Translate(type);
                Add(typeToAdd);
            } 
        }

        protected BasicBlock()
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

        private void Add(int type)
        {
            if ((blockType & type) == 0)
            {
                blockType = (blockType | type);
            }
        }
    }


}
