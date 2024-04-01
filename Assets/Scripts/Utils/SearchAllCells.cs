using System;
using System.Collections.Generic;
using Grid;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils
{
    public class SearchAllCells
    {

        private List<Cell> visitedCells = new();
        private Func<Cell, bool> InvalidCellPredicate; 
        
        public List<Cell> FindAllCells(Cell startingCell, Func<Cell, bool> invalidCellPredicate)
        {
            InvalidCellPredicate = invalidCellPredicate;
            List<Cell> addedCells = new();
            addedCells.Add(startingCell);

            bool hasReachableCells = true;

            AddNeighbors(addedCells, startingCell); 
            while (addedCells.Count > 0)
            {
                AddNeighbors(addedCells, addedCells[0]);
            }
            DebugCells(visitedCells);
            return visitedCells;
        }

        private void AddNeighbors(List<Cell> listCells, Cell origin)
        {
            List<Cell> cellsInRadius = TilingGrid.grid.GetCellsInRadius(origin, 1);
            foreach (var cell in cellsInRadius)
            {
                if (!InvalidCellPredicate.Invoke(cell))
                {
                    if (!visitedCells.Contains(cell))
                    {
                        visitedCells.Add(cell); 
                        listCells.Add(cell);
                    }
                    else
                    {
                        listCells.Remove(cell);
                    }
                }
                
            }
        }

        private static void DebugCells(List<Cell> cells)
        {
            foreach (var cell in cells)
            {
                Debug.Log(cell.position);
            }
        }
    }
}