using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/SecondQuadraSpawnerMath2")]
public class SecondQuadraSpawnerMath2 :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn <= 10)
        {
            return (int)Math.Ceiling((turn * 0.61) / 2);
        }
        else
        {
            return 0;
        }
    }

    public override int GetBigGuyToSpawn(int turn)
    {
            return 0;
    }

    public override int GetDoggoToSpawn(int turn)
    {
        if (turn <= 10)
        {
            return (int)Math.Round(turn * 0.15);
        }
        else
        {
            return 0;
        }
    }

    public override int GetSnipperToSpawn(int turn)
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
}
