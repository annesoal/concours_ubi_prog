using UnityEngine;
using UnityEditor;

namespace Grid
{
    // Il faudrait creer des Helpers pour chaque type de GameObject qui aurait besoin de ca 
    // ex : Joueur aurait des tiles valides pour le deplacement que ennemies n'auraient pas ! 
    public abstract class GridHelper
    {
        protected Cell currentCell;

        public Cell Cell
        {
            get
            {
                return currentCell;
            }
        }

        public GridHelper(Vector2Int position)
        {
            currentCell = TilingGrid.grid.GetCell(position);
        }
        public abstract bool IsValidTile(Vector2Int direction);
        public abstract Vector2Int GetHelperPosition();
        public abstract void SetHelperPosition(Vector2Int direction);
        
    }
}