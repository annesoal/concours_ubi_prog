using Grid.Blocks;
using UnityEngine;

namespace Grid
{
    public class SelectorGridHelper : GridHelper
    {
        // Permetterait de parcourir la File pour avoir les deplacements qui se jouent dans l'ordre durant la phase 
        // d'action
        private CellRecorder _recorder;
        
        public SelectorGridHelper(Vector2Int position2d) : base(position2d)
        {
            currentCell = TilingGrid.grid.GetCell(position2d);
        }
        
        public SelectorGridHelper(Vector2Int position, CellRecorder recorder) : base(position)
        {
            _recorder = recorder;
            _recorder.AddCell(currentCell);
        }

        
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

        public override void AddOnTopCell(GameObject gameObject)
        {
            currentCell.AddGameObject(gameObject);
        }


        
    }
}