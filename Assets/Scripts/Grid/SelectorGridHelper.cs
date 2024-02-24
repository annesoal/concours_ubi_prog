using Grid.Blocks;
using UnityEngine;

namespace Grid
{
    public class SelectorGridHelper : GridHelper
    {
        // Permetterait de parcourir la File pour avoir les deplacements qui se jouent dans l'ordre durant la phase 
        // d'action
        private CellRecorder _recorder = new();
        public override bool IsValidCell(Vector2Int position)
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
            _recorder.AddCell(currentCell);
        }

        public SelectorGridHelper(Vector2Int position) : base(position)
        {
            _recorder.AddCell(currentCell);
        }
    }
}