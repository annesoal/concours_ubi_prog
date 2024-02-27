using Grid.Blocks;
using UnityEngine;

namespace Grid
{
    public class ObstacleGridHelper:GridHelper
    {
        public ObstacleGridHelper(Vector2Int position) : base(position)
        {
        }

        public override bool IsValidCell(Vector2Int position)
        {
          Cell cell = TilingGrid.grid.GetCell(position);
          return (cell.type & BlockType.Walkable) > 0;
        }

        public override Vector2Int GetHelperPosition()
        {
            return currentCell.position;
        }

        public override void SetHelperPosition(Vector2Int position)
        {
            currentCell.position = position;
        }
    }
}