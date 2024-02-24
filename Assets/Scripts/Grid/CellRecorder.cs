using System.Collections.Generic;

namespace Grid
{
    public class CellRecorder
    {
        private LinkedList<Cell> _cells;

        public void AddCell(Cell cell)
        {
            _cells.AddFirst(cell);
        }

        public Cell Remove()
        {
            Cell cell = _cells.Last.Value; 
            _cells.RemoveLast();
            return cell;
        }
        
        public bool IsEmpty()
        {
            return _cells.Count < 1;
        }
    }
}