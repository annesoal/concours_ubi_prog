using System;
using Grid.Blocks;
using Unity.Collections;
using UnityEngine;

namespace Grid
{
    public class TilingGrid : MonoBehaviour 
    {
        // A changer au besoin
        static public TilingGrid grid;
        private const int Size = 100; 
        private Cell [,] _cells = new Cell[Size, Size];
        private IBlock _blocks; 
        // Start is called before the first frame update
        void Start()
        {
            GameObject ground = GameObject.Find("Ground");
            IBlock[] blocks = ground.GetComponentsInChildren<IBlock>();

            foreach (var block in blocks)
            {
                AddBlockAsCell(block);
            }
            grid = this;
            DebugCells();
        }
        // Ajoute un bloc dans la liste de Cells
        private void AddBlockAsCell(IBlock block)
        {
            Cell cell = new Cell();
            cell.type = block.GetBlockType();
            Vector2Int position = LocalToGridPosition(block.GetPosition());
            // TODO : Refactor? car duplication de l'information
            cell.position = position;
            _cells[position.x, position.y] = cell;
        
        }
        
        // Traduit une position local a la position dans la grille 
        public static Vector2Int LocalToGridPosition(Vector3 position)
        {
            Vector2Int gridPosition = new Vector2Int(); 
            gridPosition.x = (int)position.x; 
            gridPosition.y = (int)position.z; 
            return gridPosition;
        }
        
        // Donne la Cellule a la position donnee.
        public Cell GetCell(Vector2Int position) 
        {
            if (position.x >= Size || position.y >= Size)
                throw new ArgumentException();
            
            return _cells[position.x,position.y];
        }
        // Traduit une position dans la grille a une position local
        public static Vector3 GridPositionToLocal(Vector2Int position)
        {
            Vector3 localPosition = new Vector3(); 
            localPosition.x = position.x;
            localPosition.y = 0.51f; // Devrait toujours etre 0 pour la position locale TODO : Changer ca ! 
            localPosition.z = position.y; 
            return localPosition;
        }

        //-----------------------------------------------------------------------------------------------------------------
        // Fonctions utilitaires pour le deboggage ! 
        void DebugCells()
        {
            foreach (var cell in _cells)
            { 
                if (cell.type == BlockType.Walkable)
                    Debug.Log("type : " + cell.type + " a " + cell.position.x +  ", " + cell.position.y); 
            }
        }
    }
}
