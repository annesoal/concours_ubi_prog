using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/SecondQuadraSpawnerMath2")]
public class SecondQuadraSpawnerMath2 : MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn > TowerDefenseManager.TotalRounds)
            return 0;
        return (int)Math.Ceiling(turn * 0.61 / 2);
    }

    public override int GetBigGuyToSpawn(int turn)
    {
        if (turn > TowerDefenseManager.TotalRounds)
            return 0;
        return 0;
    }

    public override int GetDoggoToSpawn(int turn)
    {
        if (turn > TowerDefenseManager.TotalRounds)
            return 0;
        return (int)Math.Round(turn * 0.15);
    }

    public override int GetSnipperToSpawn(int turn)
    {
        if (turn > TowerDefenseManager.TotalRounds)
            return 0;
        return (int)Math.Round(turn * 0.1);
    }
}