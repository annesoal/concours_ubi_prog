using System.Collections.Generic;
using Grid;
using Unity.Netcode;
using UnityEngine;


public class SpawnMalus : NetworkBehaviour
{
    private static float _overTheTiles = 0.5f;
    private Dictionary<Vector2Int, int> positionsPlayer;
    public static List<Cell> _reachableCells = new List<Cell>();

    [SerializeField] private GameObject malus;


    void RegisterCellForMalus(object sender, Vector2Int positionCellPlayer)
    {
        if (positionsPlayer is null)
        {
            positionsPlayer = new Dictionary<Vector2Int, int>();
        }

        if (positionsPlayer.ContainsKey(positionCellPlayer))
        {
            positionsPlayer[positionCellPlayer]++;
        }
        else
        {
            positionsPlayer.Add(positionCellPlayer, 1);
        }
    }


    public void SpawnMalusOnGridPlayers()
    {
        Vector2Int mostUsedCellMonkey = GetMostUsedCell(true);
        Vector2Int mostUsedCellRobot = GetMostUsedCell(false);

        SpawnMalusOnCell(mostUsedCellMonkey);
        SpawnMalusOnCell(mostUsedCellRobot);
        
        
    }

    private void SpawnMalusOnCell(Vector2Int mostUsedCell)
    {
        if (mostUsedCell == Vector2Int.zero)
        {
            Debug.LogError("Aucun malus spawn pour monkey");
        }
        else
        {
            PlaceMalus(mostUsedCell);
        }
    }


    private bool IsPlayerCell(Vector2Int position, List<Cell> players)
    {
        foreach (var cellPlayer in players)
        {
            if (position == cellPlayer.position)
                return true;
        }

        return false;
    }

    private Vector2Int GetMostUsedCell(bool isMonkey)
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

        foreach (var keyValue in positionsPlayer)
        {
            if (keyValue.Value > maxOccurence && IsPlayerCell(keyValue.Key, _reachableCells))
            {
                mostUsedCell = keyValue.Key;
            }
        }

        return mostUsedCell;
    }

    //voir PlaceObjects de spawnermanager
    private void PlaceMalus(Vector2Int positionOfSpawn)
    {
        Cell cell = TilingGrid.grid.GetCell(positionOfSpawn);
        if (isValidCell(cell.position))
        {
            //event ? si pas valide ?
            GameObject instance = Instantiate(malus);
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(instance, positionOfSpawn);
            instance.GetComponent<NetworkObject>().Spawn(true);
        }
      
    }

    private bool isValidCell(Vector2Int cellToCheck)
    {
        //TODO pas de ressource
        // pas de bonus
        //pas de player
        // pas rien
        return true; 
    }
}