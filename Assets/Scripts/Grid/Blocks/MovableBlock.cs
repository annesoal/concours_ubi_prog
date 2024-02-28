namespace Grid.Blocks
{
    public class MovableBlock :BasicBlock
    {
        public MovableBlock()
        {
            blockType = BlockType.Movable | BlockType.Walkable;
        }
    }
}