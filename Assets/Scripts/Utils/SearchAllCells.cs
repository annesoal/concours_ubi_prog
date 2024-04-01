using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Utils
{
    public class SearchAllCells
    {
        private Dictionary<Cell, bool> visitedCells = new();
        private Func<Cell, bool> InvalidCellPredicate; 
        
        public List<Cell> FindAllCells(Cell startingCell, Func<Cell, bool> invalidCellPredicate)
        {
            InvalidCellPredicate = invalidCellPredicate;
            List<Cell> addedCells = new();
            visitedCells.Add(startingCell, false);

            AddNeighbors(addedCells, startingCell); 
            while (addedCells.Count > 0)
            {
                Cell[] currentAddedCells = addedCells.ToArray();
                foreach (var cell in currentAddedCells)
                {
                    AddNeighbors(addedCells, cell);
                }
            }

            List<Cell> cells = new List<Cell>();
            foreach (var cellToAdd in visitedCells)
            {
                cells.Add(cellToAdd.Key);
            }

            return cells;
        }

        private void AddNeighbors(List<Cell> listCells, Cell origin)
        {
            visitedCells[origin] = true;
            List<Cell> cellsInRadius = TilingGrid.grid.GetCellsInRadius(origin, 1);
            foreach (var cell in cellsInRadius)
            {
                if (!InvalidCellPredicate.Invoke(cell))
                {
                    if (!visitedCells.ContainsKey(cell))
                    {
                        visitedCells.Add(cell, false); 
                        listCells.Add(cell);
                    }
                    else
                    {
                        if (visitedCells[cell])
                        {
                            listCells.Remove(cell);
                        }
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