using System;
using Managers;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/FourthQuadraSpawnerMath2")]
public class FourthQuadraSpawnerMath2 : MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Ceiling(turn * 0.28);

    }

    public override int GetBigGuyToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Round(turn * 0.1);
    }

    public override int GetDoggoToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Round(turn * 0.15);
    }

    public override int GetSnipperToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return 0;
    }
}