using System.Collections.Generic;
using Grid;
using Unity.Netcode;
using UnityEngine;


public class SpawnMalus : NetworkBehaviour
{

    private Dictionary<Vector2Int, int> positionsPlayer;
    public static List<Cell> _monkeyReachableCells = new List<Cell>();
    public static List<Cell> _robotReachableCells = new List<Cell>();

    
    
    
    
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



    public void SpawnMalusOnGridPlayers(Vector2Int positionToSpawn, bool isMonkey)
    {
        
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
}


