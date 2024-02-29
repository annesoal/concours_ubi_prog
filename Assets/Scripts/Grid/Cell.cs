using System.Collections.Generic;
using Grid.Blocks;
using UnityEngine;

namespace Grid
{
    public struct Cell
    {
        public int type;
        public Vector2Int position;
        public List<GameObject> objectsOnTop;
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
        
        public void AddGameObject(GameObject gameObject)
        {
            if (objectsOnTop == null)
                objectsOnTop = new List<GameObject>();

            objectsOnTop.Add(gameObject);
        }
        
    }
    
    public static class BlockType
    {
        public const int None =         0b0000_0000_0000_0000;
        public const int Walkable =     0b0000_0000_0000_0001;
        public const int Buildable =    0b0000_0000_0000_0010;
        public const int Movable =      0b0000_0000_0000_0100;
        public const int Destructible = 0b0000_0000_0000_1000;
        public const int SpawnBlock1 =  0b0000_0000_0001_0000;
        public const int SpawnBlock2 =  0b0000_0000_0010_0000;
        public const int BasicBlock =   0b0000_0000_0100_0000; 
    }
}