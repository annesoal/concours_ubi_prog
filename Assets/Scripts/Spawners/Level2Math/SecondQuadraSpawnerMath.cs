using System;
using Managers;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/SecondQuadraSpawnerMath")]
public class SecondQuadraSpawnerMath : MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Ceiling(turn * 0.55 / 4);

    }

    public override int GetBigGuyToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return 0;
    }

    public override int GetDoggoToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Round(turn * 0.35 * 0.75 / 2);

    }

    public override int GetSnipperToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Round(turn * 0.1 / 2);

    }
}