using UnityEngine;


public class SpawnMalus : NetworkBehaviour
{

    private IDictionary<Vector2Int, int> positionsPlayer;

    public event EventHandler EventTempo;

    void Awake()
    {
        EventTempo += RegisterCellForMalus;
    }


    void RegisterCellForMalus(object sender, Vector2Int positionCellPlayer)
    {
        if (positionsPlayer is null)
        {
            positionsPlayer = new IDictionary<Vector2Int, int>
        }

        if (keyValueDictionary.ContainsKey(positionCellPlayer))
        {
            keyValueDictionary[positionCellPlayer]++;
        }
        else
        {

            keyValueDictionary.Add(positionCellPlayer, 1);
        }

    }

    public Vector2Int GetMostUsedCell()
    {

        Vector2Int maxKey = Vector2Int.zero;
        int maxValue = int.MinValue;

        foreach (var kvp in keyValueDictionary)
        {
            if (kvp.Value > maxValue)
            {
                maxKey = kvp.Key;
                maxValue = kvp.Value;
            }
        }

        return maxKey;
    }
}


