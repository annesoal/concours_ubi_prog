using System.Collections.Generic;
using Grid;
using Unity.Netcode;
using UnityEngine;


public class SpawnMalus : NetworkBehaviour
{
    private static float _overTheTiles = 0.5f;
    private Dictionary<Vector2Int, int> positionsPlayer;
    public static List<Cell> _monkeyReachableCells = new List<Cell>();
    public static List<Cell> _robotReachableCells = new List<Cell>();

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

        if (mostUsedCellMonkey == Vector2Int.zero)
        {
            Debug.LogError("Aucun malus spawn pour monkey");
            return;
        }
        PlaceMalus(mostUsedCellMonkey);


        if (mostUsedCellRobot == Vector2Int.zero)
        {
            Debug.LogError("Aucun malus spawn pour robot");
            return;
        }

        PlaceMalus(mostUsedCellRobot);


    }

    
    
    public Vector2Int GetMostUsedCell(bool isMonkey)
    {
        Vector2Int mostUsedCell = Vector2Int.zero;
        int maxOccurence = 0;
        if (isMonkey)
        {
           
            _monkeyReachableCells = TilingGrid.GetMonkeyReachableCells();
           
            foreach (var keyValue in positionsPlayer)
            {
                if (keyValue.Value > maxOccurence)
                {
                    mostUsedCell = keyValue.Key;
                }
            }

            return mostUsedCell;

        }
        
        _monkeyReachableCells = TilingGrid.GetRobotReachableCells();
 
        foreach (var keyValue in positionsPlayer)
        {
            if (keyValue.Value > maxOccurence)
            {
                mostUsedCell = keyValue.Key;
            }
                
        }

        return mostUsedCell;
       
    }
    
    //PlaceObjects de spawnermanager
    private void PlaceMalus(Vector2Int positionOfSpawn)
    {
        Cell cell = TilingGrid.grid.GetCell(positionOfSpawn);
        //event ?
        GameObject instance = Instantiate(malus);
        TilingGrid.grid.PlaceObjectAtPositionOnGrid(instance, positionOfSpawn);
        instance.GetComponent<NetworkObject>().Spawn(true);
        
        
    }

    private bool isValidCell(Vector2Int cellToCheck)
    {
        return true; //TODO
    }
}


