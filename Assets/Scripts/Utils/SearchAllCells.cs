using System;
using System.Collections.Generic;
using Grid;
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

            AddNeighbors(addedCells, startingCell); 
            while (addedCells.Count > 0)
            {
                Cell[] currentAddedCells = addedCells.ToArray();
                foreach (var cell in currentAddedCells)
                {
                    AddNeighbors(addedCells, cell);
                }
            }
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
                else
                {
                    Debug.Log(cell.position + " not valid");
                }
            }
        }

 
    }
}