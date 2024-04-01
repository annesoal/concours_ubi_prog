using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEngine;

namespace Utils
{
    public class AStarPathfinding
    {
        public static List<Cell> GetPath(Cell origin, Cell destination, Func<Cell, bool> cellValidator)
        {
            // Variables 
            List<Cell> openSet = new List<Cell>();
            Dictionary<Cell, Cell> cameFrom = new(); // K : current, V : previous
            Dictionary<Cell, float> originDistance = new(); // distance depuis origin
            Dictionary<Cell, float> destinationDistance = new(); // distance depuis destination

            // introduire origin dans les variables
  
            openSet.Add(origin);
       
            originDistance.Add(origin, 0.0f);
            destinationDistance.Add(origin, Cell.Distance(origin, destination));
           
            while (openSet.Count > 0)
            {
                
                Cell current = GetMinFScoreCell(openSet, destinationDistance);
               
                if (current.Equals(destination))
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(current);

                List<Cell> neighbors = TilingGrid.grid.GetCellsInRadius(current, 1);
              
                foreach (var neighbor in neighbors)
                {
                    if (cellValidator(neighbor)) continue;

                    float tentativeDistanceOrigin = originDistance[current] + Cell.Distance(current, neighbor);
                    float neighborDistanceOrigin = GScoreNeighbor(originDistance, neighbor);
                    if (tentativeDistanceOrigin < neighborDistanceOrigin)
                    {
                        bool wasAdded = cameFrom.TryAdd(neighbor, current);
                        if (!wasAdded)
                        {
                            cameFrom[neighbor] = current;
                        }

                        originDistance[neighbor] = tentativeDistanceOrigin;
                        destinationDistance[neighbor] = tentativeDistanceOrigin + Cell.Distance(neighbor, destination);
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
            //TODO changer!


            openSet.Add(origin);
          //  Debug.Log("STAR 10");
          //  Debug.Log("STAR 10" + openSet[0].position);
          //  Debug.Log("STAR 11");
           // Debug.Log("STAR 11 count " + openSet.Count);
           // Debug.Log("STAR 11" + openSet[1].position);
            return openSet;
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

        private static Cell GetMinFScoreCell(List<Cell> cells, Dictionary<Cell, float> distancesDestination)
        {
            //Boucle debug pour afficher valeurs dictionnaires
            foreach (var kvp in distancesDestination)
            {
                Debug.Log($"Clé : {kvp.Key}, Valeur : {kvp.Value}");
            }

            int indexSmallest = 0;
            float minDistanceDestination = distancesDestination[cells[0]];
            Debug.Log("STAR distancesDestination[cells[0]] : " + distancesDestination[cells[0]]);
            Debug.Log("STAR minDistanceDestination : " + minDistanceDestination);
            Debug.Log("STAR CELL count : " + cells.Count);
            for (int i = 0; i < cells.Count; i++)
            {
                Cell currentCell = cells[i];
                Debug.Log("STAR currentCell boucle i : " + i + currentCell.position);
                float currentDistanceDestionation = distancesDestination[currentCell];
                
                Debug.Log("STAR currentDistanceDestionation : " + currentDistanceDestionation);
                
                if (currentDistanceDestionation < minDistanceDestination)
                {
                    minDistanceDestination = currentDistanceDestionation;
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