using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

/**
 * Classe d'exemple d'une tour spécifique.
 *
 * Une tour spécifique hérite de la classe BaseTower.
 * La classe implémente les fonctions spécifiques à la tour.
 */
public class BasicTower : BaseTower
{
    [SerializeField] private int shootingRange;
    
    public override void Build(Vector2Int positionToBuild)
    {
        towerVisuals.HidePreview();
        
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, positionToBuild);
        
        RegisterTower(this);
    }

    public override BuildableObjectSO GetBuildableObjectSO()
    {
        return buildableObjectSO;
    }
    
    public override void PlayTurn()
    {
        Debug.Log("Tour basique joue son tour");
        List<Cell> targetedEnemiesCells = TargetEnemies();
    }

    protected override List<Cell> TargetEnemies()
    {
        List<Cell> cellsInShootingRange = GetCellsInShootingRange();
        throw new System.NotImplementedException();
    }

    private const int MINIMUM_RANGE_OF_TOWER = 1;
    private List<Cell> GetCellsInShootingRange()
    {
        List<Cell> cellsInShootingRange = new List<Cell>();
        Vector2Int thisGridPosition = TilingGrid.LocalToGridPosition(transform.position);
        
        for (int i = MINIMUM_RANGE_OF_TOWER; i <= shootingRange; i++)
        {
            AddCrossCellsToList(i, thisGridPosition, ref cellsInShootingRange);
            
            // diagonale horizontale
            for (int j = 1; j < (i + 1); j++)
            {
                AddHorizontalDiagonalToList(i, j, thisGridPosition, ref cellsInShootingRange);
            }
            
            // diagonale verticale
            for (int j = 1; j < i; j++)
            {
                AddVerticalDiagonalToList(i, j, thisGridPosition, ref cellsInShootingRange);
            }
        }

        return cellsInShootingRange;
    }

    /// <summary>
    /// Cross is top, down, left, right
    /// </summary>
    private void AddCrossCellsToList(int rangeIteration, Vector2Int thisGridPosition, ref List<Cell> receiver)
    {
        AddCellAtPositionToList(thisGridPosition + Vector2Int.right * rangeIteration, ref receiver);
            
        AddCellAtPositionToList(thisGridPosition + Vector2Int.left * rangeIteration, ref receiver);
            
        AddCellAtPositionToList(thisGridPosition + Vector2Int.up * rangeIteration, ref receiver);
            
        AddCellAtPositionToList(thisGridPosition + Vector2Int.down * rangeIteration, ref receiver);
    }

    private void AddHorizontalDiagonalToList(int i, int j, Vector2Int thisGridPosition, ref List<Cell> receiver)
    {
        // diagonale horizontale haut
        AddCellAtPositionToList(thisGridPosition + Vector2Int.up * i + Vector2Int.right * j, ref receiver);
        AddCellAtPositionToList(thisGridPosition + Vector2Int.up * i + Vector2Int.left * j, ref receiver);
            
        // diagonale horizontale bas
        AddCellAtPositionToList(thisGridPosition + Vector2Int.down * i + Vector2Int.right * j, ref receiver);
        AddCellAtPositionToList(thisGridPosition + Vector2Int.down * i + Vector2Int.left * j, ref receiver);
    }

    private void AddVerticalDiagonalToList(int i, int j, Vector2Int thisGridPosition, ref List<Cell> receiver)
    {
        AddCellAtPositionToList(thisGridPosition + Vector2Int.right * i + Vector2Int.up * j, ref receiver);
        AddCellAtPositionToList(thisGridPosition + Vector2Int.right * i + Vector2Int.down * j, ref receiver);
        
        AddCellAtPositionToList(thisGridPosition + Vector2Int.left * i + Vector2Int.up * j, ref receiver);
        AddCellAtPositionToList(thisGridPosition + Vector2Int.left * i + Vector2Int.down * j, ref receiver);
    }

    private void AddCellAtPositionToList(Vector2Int positionOfCell, ref List<Cell> receiver)
    {
        receiver.Add(TilingGrid.grid.GetCell(positionOfCell));
    }
}
