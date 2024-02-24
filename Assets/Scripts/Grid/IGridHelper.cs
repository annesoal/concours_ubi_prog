using Grid.Blocks;
using UnityEngine;

namespace Grid
{
    // Il faudrait creer des Helpers pour chaque type de GameObject qui aurait besoin de ca 
    // ex : Joueur aurait des tiles valides pour le deplacement que ennemies n'auraient pas ! 
    public interface IGridHelper
    {
        bool IsValidTile(Vector3 position);
        IBlock GetBlock(Vector3 position);
        Vector3 GetPosition(IBlock block); 
    }
}