using Grid;
using UnityEngine;
using UnityEngine.UIElements;

public class GridLayout : MonoBehaviour
{
    // A changer au besoin
    private const int Size = 100; 
    private Cell [,] _cells = new Cell[Size, Size];

    private Block _blocks; 
    // Start is called before the first frame update
    void Start()
    {
       GameObject ground = GameObject.Find("Ground");
       Block[] blocks = ground.GetComponentsInChildren<Block>();

       foreach (var block in blocks)
       {
           AddBlockAsCell(block);
       }
       
        DebugCells();
    }

    private void AddBlockAsCell(Block block)
    {
        Cell cell = new Cell(); 
        // Check le type de bloc pour definir le type de cell 
        switch (block)
        {
            case BasicBlock :
                cell.type = CellType.Basic;
                break;
        }

        Vector2Int position = TranslateLocalPositionToGridPosition(block.getPosition());
        _cells[position.x, position.y] = cell;
        
    }

    // Traduit une position local a la position dans la grille 
    public Vector2Int TranslateLocalPositionToGridPosition(Vector3 position)
    {
        Vector2Int gridPosition = new Vector2Int(); 
        gridPosition.x = (int)position.x; 
        gridPosition.y = (int)position.z; 
        return gridPosition;
    }
    
    
    // Traduit une position dans la grille a une position local
    public Vector3 TranslateLocalPositionToGridPosition(Vector2 position)
    {
        Vector3 localPosition = new Vector3(); 
        localPosition.x = (int)position.x;
        localPosition.y = 0; // Devrait toujours etre 0 pour la position locale
        localPosition.z = (int)position.y; 
        return localPosition;
    }
    //-----------------------------------------------------------------------------------------------------------------
    // Fonctions utilitaires pour le deboggage ! 
    void DebugCells()
    {
        foreach (var cell in _cells)
        {
           Debug.Log("type : " + cell.type + " a " + cell.position.x +  ", " + cell.position.y); 
        }
    }
}
