using UnityEngine;

namespace Grid
{
    
    struct Cell
    {
        public CellType type;
        public Vector2Int position;
    }
    
    public enum CellType
    {
        Empty = 0,
        Basic,  
    }
}