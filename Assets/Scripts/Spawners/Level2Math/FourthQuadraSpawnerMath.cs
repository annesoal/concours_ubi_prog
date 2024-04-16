using System;
using Managers;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/FourthQuadraSpawnerMath")]
public class FourthQuadraSpawnerMath : MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Ceiling(turn * 0.14);

    }

    public override int GetBigGuyToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Round(turn * 0.3 / 2.4);
    }

    public override int GetDoggoToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return (int)Math.Round(turn * 0.05);

    }

    public override int GetSnipperToSpawn(int turn)
    {
        if (turn > EnemySpawnerManager.TotalRounds)
            return 0;
        return 0;
    }
}