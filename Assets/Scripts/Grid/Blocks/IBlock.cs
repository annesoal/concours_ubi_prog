using UnityEngine;

namespace Grid.Blocks
{
    public interface IBlock
    {
       Vector3 GetPosition();
       BlockType GetBlockType();
    }
}