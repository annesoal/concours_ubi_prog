using UnityEngine;

namespace Grid
{
    public class TilingGrid : MonoBehaviour 
    {
        // A changer au besoin
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
            
            DebugCells();
        }
        // Ajoute un bloc dans la liste de Cells
        private void AddBlockAsCell(IBlock block)
        {
            Cell cell = new Cell(); 
            // Check le type de bloc pour definir le type de cell 
            switch (block)
            {
                case BasicBlock :
                    cell.type = CellType.Basic;
                    break;
            }

            Vector2Int position = LocalToCell(block.GetPosition());
            // TODO : Refactor car duplication de l'information
            cell.position = position;
            _cells[position.x, position.y] = cell;
        
        }

        // Traduit une position local a la position dans la grille 
        public Vector2Int LocalToCell(Vector3 position)
        {
            Vector2Int gridPosition = new Vector2Int(); 
            gridPosition.x = (int)position.x; 
            gridPosition.y = (int)position.z; 
            return gridPosition;
        }
    
    
        // Traduit une position dans la grille a une position local
        public Vector3 CellToLocal(Vector2Int position)
        {
            Vector3 localPosition = new Vector3(); 
            localPosition.x = position.x;
            localPosition.y = 0; // Devrait toujours etre 0 pour la position locale
            localPosition.z = position.y; 
            return localPosition;
        }
        //-----------------------------------------------------------------------------------------------------------------
        // Fonctions utilitaires pour le deboggage ! 
        void DebugCells()
        {
            foreach (var cell in _cells)
            { 
                if (cell.type != CellType.Empty)
                    Debug.Log("type : " + cell.type + " a " + cell.position.x +  ", " + cell.position.y); 
            }
        }
    }
}
