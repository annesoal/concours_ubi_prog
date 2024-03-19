using System;
using System.Collections.Generic;
using Grid.Blocks;
using Grid.Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Grid
{
    public struct Cell
    {
        public int type;
        public Vector2Int position;
        public List<GameObject> objectsOnTop;
        public List<ITopOfCell> objectsTopOfCell; 
        
        // Check que le type soit le meme (exactement) que le type de la cellule
        public bool IsOf(int blockType)
        {
             return (this.type ^ blockType) == 0;
        }

        // Check que le type existe dans le type de la cellule
        // None donne toujours vrai
        public bool Has(int blockType)
        {
            if (blockType == BlockType.None)
                return true; 
            
            return (this.type & blockType) > 0;
        }
        
        public bool IsNone()
        {
            return this.type == 0;
        }

        public void AddGameObject(GameObject gameObject, ITopOfCell objectTopOfCell = null)
        {
            if (objectsOnTop == null)
            {
                objectsOnTop = new List<GameObject>();
            }
            objectsOnTop.Add(gameObject);

            if (objectsTopOfCell == null)
            {
                objectsTopOfCell = new();
            }

            objectsTopOfCell?.Add(objectTopOfCell);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            
        }
    }
    
    public static class BlockType
    {
        public const int None =         0b0000_0000_0000_0000;
        public const int Walkable =     0b0000_0000_0000_0001;
        public const int Buildable =    0b0000_0000_0000_0010;
        public const int Movable =      0b0000_0000_0000_0100;
        public const int EnemySpawnBlock = 0b0000_0000_0000_1000;
        public const int SpawnBlock =  0b0000_0000_0001_0000;
        public const int BasicBlock =   0b0000_0000_0100_0000; 
        public const int EnnemyWalkable =   0b0000_0000_1000_0000; 
        
        public static int Translate(Type type)
        {
            switch (type)
            {
                case Type.Walkable:
                    return BlockType.Walkable; 
                case Type.Buildable:
                    return BlockType.Buildable; 
                case Type.Movable:
                    return BlockType.Movable; 
                case Type.EnemySpawnBlock:
                    return BlockType.EnemySpawnBlock; 
                case Type.SpawnBlock:
                    return BlockType.SpawnBlock; 
                case Type.BasicBlock:
                    return BlockType.BasicBlock; 
                case Type.EnemyWalkable:
                    return BlockType.EnnemyWalkable;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
    
    public enum Type
    {
         Walkable,
         Buildable,
         Movable,
         SpawnBlock,
         BasicBlock,
         EnemyWalkable,
         EnemySpawnBlock,
    }
}