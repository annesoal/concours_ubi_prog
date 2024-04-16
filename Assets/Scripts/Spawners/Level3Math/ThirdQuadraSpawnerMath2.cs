using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/ThirdQuadraSpawnerMath2")]
public class ThirdQuadraSpawnerMath2 :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn <= 10)
        {
            return (int) Math.Ceiling((turn + 1) * 0.2);
        }
        else
        {
            return 0;
        }

    }

    public override int GetBigGuyToSpawn(int turn)
    {
        if (turn <= 10)
        {
            return (int)Math.Round(turn *0.1);
        }
        else
        {
            return 0;
        }
    }

    public override int GetDoggoToSpawn(int turn)
    {
        return 0;
    }

    public override int GetSnipperToSpawn(int turn)
    {
        if (turn <= 10)
        {
            return (int)Math.Ceiling((turn * 0.15));
        }
        else
        {
            return 0;
        }
    }
}
