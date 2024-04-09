using System.Collections.Generic;
using Grid;
using Grid.Interface;
using Unity.Netcode;
using UnityEngine;


public class SpawnMalus : NetworkBehaviour
{
    private static float _overTheTiles = 0.5f;
    private static Dictionary<Vector2Int, int> positionsPlayer;
    public static List<Cell> _reachableCells = new List<Cell>();

    public static SpawnMalus Instance { get; private set; }


    //compte le nombre de deplacementa des joueurs par position de Cell
    public static void RegisterCellForMalus(Vector2Int positionCellPlayer)
    {
        Debug.Log("Regsiter position player " + positionCellPlayer);
        if (positionsPlayer is null)
        {
            positionsPlayer = new Dictionary<Vector2Int, int>();
            Debug.Log("Register etati null " + positionsPlayer);
        }

        if (positionsPlayer.ContainsKey(positionCellPlayer))
        {
            positionsPlayer[positionCellPlayer]++;
        }
        else
        {
            positionsPlayer.Add(positionCellPlayer, 1);
        }

        Debug.Log("position Register " + positionsPlayer[positionCellPlayer]);
    }


    public static void SpawnMalusOnGridPlayers(GameObject malus)
    {
        Vector2Int mostUsedCellMonkey = GetMostUsedCell(true);
        if (mostUsedCellMonkey != Vector2Int.zero)
        {
            PlaceMalus(mostUsedCellMonkey, malus);
        }
        else
        {
            Debug.Log("Pas de spawn malus monkey");
        }


        Vector2Int mostUsedCellRobot = GetMostUsedCell(false);

        if (mostUsedCellRobot != Vector2Int.zero)
        {
            PlaceMalus(mostUsedCellRobot, malus);
        }
        else
        {
            Debug.Log("Pas de spawn malus robot");
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

    // TODO ajouter si aucun mouvement fait par le player
    // Retourne (0,0) si aucune cell nest trouve
    private static Vector2Int GetMostUsedCell(bool isMonkey)
    {
        Vector2Int mostUsedCell = Vector2Int.zero;
        int maxOccurence = 0;
        if (isMonkey)
        {
            _reachableCells = TilingGrid.GetMonkeyReachableCells();
        }
        else
        {
            _reachableCells = TilingGrid.GetRobotReachableCells();
        }

        Debug.LogError("position player " + positionsPlayer.Count);
        foreach (var keyValue in positionsPlayer)
        {
            Cell toCheck = TilingGrid.grid.GetCell(keyValue.Key);
            if (keyValue.Value > maxOccurence && IsPlayerCell(keyValue.Key, _reachableCells) && isValidCell(toCheck))
            {
                mostUsedCell = keyValue.Key;
            }
        }

        return mostUsedCell;
    }

    //voir PlaceObjects de spawnermanager
    private static void PlaceMalus(Vector2Int positionOfSpawn, GameObject malus)
    {
        Cell cell = TilingGrid.grid.GetCell(positionOfSpawn);

        GameObject instance = Instantiate(malus);
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(instance, positionOfSpawn);
        instance.GetComponent<NetworkObject>().Spawn(true);
    }


    //table construction ? TODO
    private static bool isValidCell(Cell toCheck)
    {
        Cell test = TilingGrid.grid.GetCell(toCheck.position);
        bool hasNoPlayer = toCheck.HasObjectOfTypeOnTop(TypeTopOfCell.Player);
        bool hasNoRessources = !TilingGrid.grid.HasTopOfCellOfType(test, TypeTopOfCell.Resource);
        bool hasNoBonus = !TilingGrid.grid.HasTopOfCellOfType(test, TypeTopOfCell.Bonus);
        bool hasNoBuilding = !toCheck.HasNonWalkableBuilding();
        bool hasNoMalus = !TilingGrid.grid.HasTopOfCellOfType(test, TypeTopOfCell.Malus);
        Debug.Log("No Player on cell ? " + hasNoPlayer);

        return hasNoPlayer && hasNoBonus && hasNoRessources && hasNoBuilding && hasNoMalus;
    }
}