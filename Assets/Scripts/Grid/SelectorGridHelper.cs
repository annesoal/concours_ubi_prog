using Grid.Blocks;
using UnityEngine;

namespace Grid
{
    public class SelectorGridHelper : GridHelper
    {
        public override bool IsValidTile(Vector2Int position)
        {
            position = currentCell.position + position;
            Cell cell = TilingGrid.grid.GetCell(position);
            
            // Comme ca on selectionne pas le vide 
            return cell.type != BlockType.None; 
        }

        public override Vector2Int GetHelperPosition()
        {
            return currentCell.position;
        }

        public override void SetHelperPosition(Vector2Int direction)
        {
            Vector2Int next = currentCell.position + direction; 
            currentCell = TilingGrid.grid.GetCell(next); 
        }

        public SelectorGridHelper(Vector2Int position) : base(position)
        {
            
        }
    }
}