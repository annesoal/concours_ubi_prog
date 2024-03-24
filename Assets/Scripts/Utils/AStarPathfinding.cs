using System;
using System.Collections.Generic;
using System.Linq;
using Grid;

namespace Utils
{
    public class AStarPathfinding
    {
        private static List<Cell> GetPath(Cell origin, Cell destination, Func<bool> cellValidator)
        {
            List<Cell> openSet = new List<Cell>();
            openSet.Add(origin);
            Dictionary<Cell, Cell> cameFrom = new(); // K : current, V : previous
            Dictionary<Cell, float> gScore = new(); // distance depuis origin
            gScore.Add(origin, 0.0f);
            Dictionary<Cell, float> fScore = new(); // distance depuis destination
            fScore.Add(origin, Cell.Distance(origin, destination));

            while (openSet.Count > 0)
            {
                Cell current = GetMinFScoreCell(openSet, fScore);
                if (current.Equals(destination))
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(current);
                List<Cell> neighbors = TilingGrid.grid.GetCellsInRadius(current,1);
                foreach (var neighbor in neighbors)
                {
                    float tentativeGScore = gScore[current] + Cell.Distance(current, neighbor);
                    float gScoreNeighbor = GScoreNeighbor(gScore, neighbor);
                    if (tentativeGScore < gScoreNeighbor)
                    {
                       cameFrom.Add(neighbor, current);
                       gScore[neighbor] = tentativeGScore;
                       fScore[neighbor] = tentativeGScore + Cell.Distance(neighbor, destination);
                       if (!openSet.Contains(neighbor))
                       {
                           openSet.Add(neighbor);
                       }
                    }
                }
            }
            //TODO changer!
            throw new Exception("Path not found");
        }

        private static float GScoreNeighbor(Dictionary<Cell, float> gScore, Cell neighbor)
        {
            float gScoreNeighbor;
            try
            {
                gScoreNeighbor = gScore[neighbor];
            }
            catch (KeyNotFoundException)
            {
                gScore.Add(neighbor, float.PositiveInfinity);
                gScoreNeighbor = float.PositiveInfinity;
            }

            return gScoreNeighbor;
        }

        private static Cell GetMinFScoreCell(List<Cell> cells, Dictionary<Cell, float> fscores)
        {
            int indexSmallest = 0; 
            float minFScore = fscores[cells[0]];
            for (int i = 0; i < cells.Count; i++)
            {
                Cell currentCell = cells[i];
                float currentFScore = fscores[currentCell];
                if (currentFScore < minFScore)
                {
                    minFScore = currentFScore;
                    indexSmallest = i;
                }
            }
            return cells[indexSmallest];
        }

        public static List<Cell> ReconstructPath(Dictionary<Cell, Cell> cameFrom, Cell current)
        {
            List<Cell> path = new List<Cell>();
            path.Add(current);
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }
            return path;
        }
    }
}