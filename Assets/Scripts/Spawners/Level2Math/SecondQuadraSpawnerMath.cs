using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/SecondQuadraSpawnerMath")]
public class SecondQuadraSpawnerMath :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        return (int)((int)((1 - Mathf.Pow(-1, turn)) / 2) * Math.Ceiling((turn * 0.55) / 2));
    }

    public override int GetBigGuyToSpawn(int turn)
    {
        return 0;
    }

    public override int GetDoggoToSpawn(int turn)
    {
        return (int)((int)((1 - Mathf.Pow(-1, turn)) / 2) * Math.Round((turn * 0.35)*0.75));
    }

    public override int GetSnipperToSpawn(int turn)
    {
        return (int)((int)((1 - Mathf.Pow(-1, turn)) / 2) * Math.Round(turn *0.1));
    }
}
