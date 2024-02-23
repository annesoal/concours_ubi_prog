using UnityEngine;

namespace Grid
{
    public class Cell
    {
        public CellType type = CellType.Empty;
        public Vector2Int position; 
    }
    
    public enum CellType
    {
        Empty = 0,
        Basic,  
    }
}