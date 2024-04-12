using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Grid.Interface;
using UnityEngine;
using Enemies;
using Unity.VisualScripting;

/**
 * Tour tirant les ennemis les plus loins d'elle sur un axe particulier.
 */
public class BasicTower : BaseTower
{
    public static int BasicTowerRange; 
    public static int BasicTowerDamage; 
    public static int BasicTowerTimeBetweenShots; 
    public static int BasicTowerHealth; 
    public static int BasicTowerProjectilesNumber;
    public static int BasicTowerCost;

    private int _range = BasicTowerRange;
    private int _damage = BasicTowerDamage;
    private int _timeBetweenShots = BasicTowerTimeBetweenShots;
    private int _health = BasicTowerHealth;
    private int _projectileNumber = BasicTowerProjectilesNumber;
    private int _cost = BasicTowerCost;
    
    
    public override int Cost { get => _cost; set => _cost = value ; }
    public override int AttackDamage { get => _damage; set => _damage = value; }
    public override int Health { get => _health; set => _health = value; }
    public override int TimeBetweenShots { get => _timeBetweenShots; set => _timeBetweenShots = value; }
    public override int Range { get => _range; set => _range = value; }
    public override int TotalOfProjectile { get => _projectileNumber; set => _projectileNumber = value; }

    protected override List<Cell> TargetEnemies()
    {
        List<Cell> cellsInShootingRange = GetCellsInShootingRange();
        
        return TargetFarthestEnemies(cellsInShootingRange);
    }

    private const int MINIMUM_RANGE_OF_TOWER = 1;
    private List<Cell> GetCellsInShootingRange()
    {
        List<Cell> cellsInShootingRange = new List<Cell>();
        Vector2Int thisGridPosition = TilingGrid.LocalToGridPosition(transform.position);
        
        for (int i = MINIMUM_RANGE_OF_TOWER; i <= Range; i++)
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

    private const int INITIAL_NUMBER_OF_TARGET_SET = 0;
    private const int NO_ENEMY_FOUND_IN_LIST = -1;
    
    private List<Cell> TargetFarthestEnemies(List<Cell> cellsInShootingRange)
    {
        int numberOfTargetSet = INITIAL_NUMBER_OF_TARGET_SET;
        List<Cell> targetedCells = new List<Cell>();

        while (numberOfTargetSet != TotalOfProjectile)
        {
            int indexOfFarthestEnemyCell = SearchIndexOfFarthestEnemyCell(cellsInShootingRange);

            if (indexOfFarthestEnemyCell == NO_ENEMY_FOUND_IN_LIST)
            {
                break;
            }
            
            targetedCells.Add(cellsInShootingRange[indexOfFarthestEnemyCell]);
            
            numberOfTargetSet++;
            Debug.Log("" + numberOfTargetSet);
        }

        return targetedCells;
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

    private int SearchIndexOfFarthestEnemyCell(List<Cell> cellsInShootingRange)
    {
        int indexOfFarthestEnemyCell = NO_ENEMY_FOUND_IN_LIST;
        Vector2Int farthestEnemyPosition = TilingGrid.LocalToGridPosition(transform.position);
            
        for (int i = cellsInShootingRange.Count - 1; i >= 0; i--)
        {
            Cell contender = cellsInShootingRange[i];
            if (CellHasTargetOnTopOfCells(contender))
            {
                if (CellDistanceIsGreater(contender, farthestEnemyPosition))
                {
                    farthestEnemyPosition = contender.position;
                    indexOfFarthestEnemyCell = i;
                }
            }
        }

        return indexOfFarthestEnemyCell;
    }

    private bool CellHasTargetOnTopOfCells(Cell cell)
    {
        return cell.ObjectsTopOfCell.Any(objectOnTop => objectOnTop.GetType() == TypeTopOfCell.Enemy);
    }
    
    /// <returns>Returns the new max distance, or DISTANCE_IS_SHORTER if distance contender is shorter</returns>
    private bool CellDistanceIsGreater(Cell contender, Vector2Int farthestEnemyPosition)
    {
        if (EarlyReturnBasedOnDirection(contender, farthestEnemyPosition))
        {
            Debug.Log("EarlyReturn!");
            return false;
        }
        
        Vector2Int thisGridPosition = TilingGrid.LocalToGridPosition(transform.position);
        
        float contenderDistance = Vector2Int.Distance(thisGridPosition, contender.position);
        float lastMaxDistance = Vector2Int.Distance(thisGridPosition, farthestEnemyPosition);
        
        if (contenderDistance > lastMaxDistance)
        {
            Debug.Log("contender >  lasMax");
            return true;
        }
        else
        {
            Debug.Log("lastMax > contender");
            return false;
        }
    }

    private bool EarlyReturnBasedOnDirection(Cell contender, Vector2Int farthestEnemyPosition)
    {
        Func<Vector3, Vector3, bool> earlyReturnPredicate = GetEarlyReturnPredicateBasedOnEnemyDirection();
        
        Vector3 contenderWorldPosition = TilingGrid.GridPositionToLocal(contender.position);
        Vector3 farthestEnemyWorldPosition = TilingGrid.GridPositionToLocal(farthestEnemyPosition);

        return earlyReturnPredicate(contenderWorldPosition, farthestEnemyWorldPosition);
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private Func<Vector3, Vector3, bool> GetEarlyReturnPredicateBasedOnEnemyDirection()
    {
        switch (enemyDirection)
        {
            // TODO : J'ai enlever les excpetions pour le moment
            case EnemyDirection.ZPositive:
                return (contenderPosition, lasMaxPosition) => contenderPosition.z < lasMaxPosition.z;
            case EnemyDirection.ZNegative:
                // throw new NotImplementedException();
                break;
            case EnemyDirection.YPositive:
                // throw new NotImplementedException();
                break;
            case EnemyDirection.YNegative:
                // throw new NotImplementedException();
                break;
            case EnemyDirection.XPositive:
                // throw new NotImplementedException();
                break;
            case EnemyDirection.XNegative:
                // throw new NotImplementedException();
                break;
            default:
                // throw new ArgumentOutOfRangeException();
                break;
        }

        return null;
    }
    
}
