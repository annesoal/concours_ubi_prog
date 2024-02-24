using UnityEngine;
using UnityEditor;

namespace Grid
{
    // Il faudrait creer des Helpers pour chaque type de GameObject qui aurait besoin de ca 
    // ex : Joueur aurait des tiles valides pour le deplacement que ennemies n'auraient pas ! 
    public abstract class GridHelper
    {
        protected Cell currentCell;

        // Simplement un Getter... 
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
        // Check si la Cell qu'on vise possede une cellule valide (On se base sur la position actuelle)
        public abstract bool IsValidCell(Vector2Int direction);
        // Donne la position du Helper sur la grille
        public abstract Vector2Int GetHelperPosition();
        // Change la position du Helper sur la grille
        public abstract void SetHelperPosition(Vector2Int direction);
        
    }
}