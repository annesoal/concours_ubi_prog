using Grid.Blocks;
using UnityEngine;

namespace Grid
{
    public class ObstacleGridHelper:GridHelper
    {
        public ObstacleGridHelper(Vector2Int position) : base(position)
        {
        }

        //TODO pas de spawn sur player
        public override bool IsValidCell(Vector2Int position)
        {
          Cell cell = TilingGrid.grid.GetCell(position);
          bool isValidBlockType = (cell.type & BlockType.Walkable) > 0;
          bool itHasPlaceOnTop = cell.objectsOnTop == null || cell.objectsOnTop.Count == 0;
          return isValidBlockType && itHasPlaceOnTop;
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