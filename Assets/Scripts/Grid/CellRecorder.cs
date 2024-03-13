using System.Collections.Generic;

namespace Grid
{
    public class CellRecorder
    {
        private LinkedList<Cell> _cells = new();

        public void AddCell(Cell cell)
        {
            _cells.AddFirst(cell);
        }

        public Cell RemoveLast()
        {
            Cell cell = _cells.Last.Value; 
            _cells.RemoveLast();
            return cell;
        }
        
        public bool IsEmpty()
        {
            return _cells.Count < 1;
        }

        public int Size()
        {
            return _cells.Count; 
        }

        public void Reset()
        {
            while (!IsEmpty())
                _cells.RemoveLast();
        }
    }
}