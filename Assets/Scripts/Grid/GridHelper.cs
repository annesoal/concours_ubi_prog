using UnityEngine;

namespace Grid
{
    // Il faudrait creer des Helpers pour chaque type de GameObject qui aurait besoin de ca 
    // ex : Joueur aurait des tiles valides pour le deplacement que ennemies n'auraient pas ! 
    public interface GridHelper
    {
        bool IsValidTile(Vector3 position);
        Block GetBlock(Vector3 position);
        Vector3 GetPosition(Block block); 
    }
}