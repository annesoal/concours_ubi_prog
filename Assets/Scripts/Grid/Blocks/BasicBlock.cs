using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Grid.Blocks
{
    public class BasicBlock : MonoBehaviour
    {
        [SerializeField] private List<Type> Types = new ();
        public int blockType;
        
        protected BasicBlock()
        {
            foreach (var type in Types)
            {
                int typeToAdd = translate(type);
                Add(typeToAdd);
            } 
        }

        public Vector3 GetPosition()
        {
            return this.transform.localPosition;
        }

        public int GetBlockType()
        {
            return blockType;
        }

        private int translate(Type type)
        {
            switch (type)
            {
                case Type.Walkable:
                    return BlockType.Walkable; 
                case Type.Buildable:
                    return BlockType.Buildable; 
                case Type.Movable:
                    return BlockType.Movable; 
                case Type.Destructible:
                    return BlockType.Destructible; 
                case Type.SpawnBlock:
                    return BlockType.SpawnBlock; 
                case Type.BasicBlock:
                    return BlockType.BasicBlock; 
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
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
