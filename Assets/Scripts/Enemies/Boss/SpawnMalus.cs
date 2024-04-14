using System;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using Managers;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;


public class SpawnMalus : NetworkBehaviour
{
    private static float _overTheTiles = 0.5f;
    private static Dictionary<Vector2Int, int> positionsPlayerRegister = new Dictionary<Vector2Int, int>();
    public static SpawnMalus Instance { get; private set; }


    //compte le nombre de deplacementa des joueurs par position de Cell
    public static void RegisterCellForMalus(Vector2Int positionCellPlayer)
    {
        if (positionsPlayerRegister.ContainsKey(positionCellPlayer))
        {
            positionsPlayerRegister[positionCellPlayer]++;
        }
        else
        {
            positionsPlayerRegister.Add(positionCellPlayer, 1);
        }

        Debug.Log("position Register " + positionsPlayerRegister[positionCellPlayer]);
    }


    public static void SpawnMalusOnGridPlayers(GameObject malus)
    {
        //sil y a eu au moins un deplacement de lun des deux players
        if (positionsPlayerRegister != null)
        {
            var mostUsedCells = GetMostUsedCells();

            //Si une ou plusieurs used cells trouve, faire spawner
            if (mostUsedCells != null && mostUsedCells.Count > 0)
            {
                Debug.Log("SPawnMAlus avant placemalus");
                PlaceMalus(mostUsedCells.ToArray(), malus, IsInvalidCell);
            }
        }
    }

    

    // retourne une liste contenant la cell la plus utilisee pour chacun des joueurs
    private static List<Vector2Int> GetMostUsedCells()
    {
        List<Vector2Int> mostUsedCells = new();
        List<Cell> reachableCellsMonkey = GetReachableCells(true);
        List<Cell> reachableCellsRobot = GetReachableCells(false);

        //most used cell de Monkey
        var mostUsedCellTemp = MostUsedCell(reachableCellsMonkey);
        if (mostUsedCellTemp != Vector2Int.zero)
            mostUsedCells.Add(mostUsedCellTemp);

        //most used cell de Robot
        mostUsedCellTemp = MostUsedCell(reachableCellsRobot);
        if (mostUsedCellTemp != Vector2Int.zero)
            mostUsedCells.Add(mostUsedCellTemp);

        return mostUsedCells;
    }
    
    private static List<Cell> GetReachableCells(bool isMonkey)
    {
        if (isMonkey)
            return TilingGrid.GetMonkeyReachableCells();
        
        return TilingGrid.GetRobotReachableCells();
    }

    private static Vector2Int MostUsedCell(List<Cell> cellsReachable)
    {
        int maxOccurence = 0;
        Vector2Int mostUsedCellTemp = Vector2Int.zero;
        
        //pour chaque position contenu dans le dictionnaire
        Debug.Log("boucle sur dictionnaire, nb elemen " + positionsPlayerRegister.Count);
        foreach (var keyValue in positionsPlayerRegister)
        {
            Cell toCheck = TilingGrid.grid.GetCell(keyValue.Key);
            
            if (keyValue.Value > maxOccurence && IsPlayerCell(keyValue.Key, cellsReachable) &&
                isValidCell(toCheck))
            {
                mostUsedCellTemp = keyValue.Key;
                maxOccurence = keyValue.Value;
            }
        }

        Debug.Log("SpawnMalus mosUsedcell " + mostUsedCellTemp);
        return mostUsedCellTemp;
    }


    private static void PlaceMalus(Vector2Int[] positionsToSpawn, GameObject gameObjectsToSpawn,
        Func<Cell, bool> isInvalidCell)
    {
        foreach (Vector2Int position in positionsToSpawn)
        {
            Debug.Log("Spawnmalus placemalus");
            GameObject instance = Instantiate(gameObjectsToSpawn);
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(instance, position);
            instance.GetComponent<NetworkObject>().Spawn(true);
        }
    }

    private static bool IsPlayerCell(Vector2Int position, List<Cell> players)
    {
        foreach (var cellPlayer in players)
        {
            if (position == cellPlayer.position)
                return true;
        }

        return false;
    }
    

    private static bool IsInvalidCell(Cell cell)
    {
        return !isValidCell(cell);
    }


    private static bool isValidCell(Cell toCheck)
    {
        Cell cellUpdated = TilingGrid.grid.GetCell(toCheck.position);
        Debug.Log("position in isValid position malus " + cellUpdated.position);
        Debug.Log("has player on top of cell " + (cellUpdated.ObjectsTopOfCell.Count > 0));

        return !(cellUpdated.ObjectsTopOfCell.Count > 0);
        {
            
        }
        foreach (TypeTopOfCell type in Enum.GetValues(typeof(TypeTopOfCell)))
        {
            bool hasType = TilingGrid.grid.HasTopOfCellOfType(cellUpdated, type);

            if (hasType)
            {
                Debug.Log($"Cell contains {type}.");
                return false;
            }
        }
        
     
        return true;
    }
}