using UnityEngine;
using UnityEditor;

namespace Grid
{
    // Il faudrait creer des Helpers pour chaque type de GameObject qui aurait besoin de ca 
    // ex : Joueur aurait des tiles valides pour le deplacement que ennemies n'auraient pas ! 
    public abstract class GridHelper
    {
        protected Cell currentCell;
        public abstract bool IsValidTile(Vector2Int position);
        public abstract Vector2Int GetHelperPosition();
        public abstract void SetHelperPosition(Vector2Int direction);
    }
}