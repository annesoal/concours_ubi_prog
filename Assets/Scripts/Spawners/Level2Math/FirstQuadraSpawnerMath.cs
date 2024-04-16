using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/FirstQuadraSpawnerMath")]
public class FirstQuadraSpawnerMath : MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn > TowerDefenseManager.TotalRounds)
            return 0;
        return 0;
    }

    public override int GetBigGuyToSpawn(int turn)
    {
        if (turn > TowerDefenseManager.TotalRounds)
            return 0;
        return (int)Math.Max(Math.Ceiling((turn - 1) * 0.1 * 0.75 / 2), 0);
    }

    public override int GetDoggoToSpawn(int turn)
    {
        if (turn > TowerDefenseManager.TotalRounds)
            return 0;
        return (int)Math.Round(turn * 0.5 * 0.75 / 2);
    }

    public override int GetSnipperToSpawn(int turn)
    {
        if (turn > TowerDefenseManager.TotalRounds)
            return 0;
        return (int)Math.Ceiling(turn * 0.2 * 0.75 / 2);
    }
}