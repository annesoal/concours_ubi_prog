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
    private List<TypeTopOfCell> blockingElementsType; //TODO fonctionne ?
    
    [SerializeField] private GameObject malus;


    //compte le nombre de deplacementa des joueurs par position de Cell
    public static void RegisterCellForMalus(Vector2Int positionCellPlayer)
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
        if (isValidCell(cell))
        {
            //event ? si pas valide ? (comme dans spawner manager)
            GameObject instance = Instantiate(malus);
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(instance, positionOfSpawn);
            instance.GetComponent<NetworkObject>().Spawn(true);
        }
      
    }

    // Tester
    private bool isValidCell(Cell cellToCheck)
    {
        foreach (var elementType in blockingElementsType)
        {
            if (cellToCheck.HasObjectOfTypeOnTop(elementType))
                return false;
        }

        return true;
    }
}