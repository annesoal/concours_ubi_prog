using System;
using Managers;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/ThirdQuadraSpawnerMath")]
public class ThirdQuadraSpawnerMath : MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Ceiling((turn + 1) * 0.1);
    }

    public override int GetBigGuyToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Max(Math.Round((turn - 1) * 0.15), 0);
    }

    public override int GetDoggoToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return 0;
    }

    public override int GetSnipperToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Ceiling(turn * 0.15 / 2);
    }
}