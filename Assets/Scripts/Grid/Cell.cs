using Grid.Blocks;
using UnityEngine;

namespace Grid
{
    public struct Cell
    {
        public int type;
        public Vector2Int position;
        
        public bool IsOf(int type)
        {
            if (type == BlockType.None)
                return this.type == 0;

            return (this.type & type) > 0;
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