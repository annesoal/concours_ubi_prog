using UnityEngine;

namespace Grid.Blocks
{
    public class SpawnBlockPlayer1 : BlockPlayerSpawn 
    {
        public SpawnBlockPlayer1()
        {
            blockType = BlockType.SpawnBlock1 | BlockType.Walkable;
        }
   
    }
}