using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/SecondQuadraSpawnerMath")]
public class SecondQuadraSpawnerMath :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        return (int)Math.Ceiling((turn * 0.55) / 2);
    }

    public override int GetBigGuyToSpawn(int turn)
    {
        return 0;
    }

    public override int GetDoggoToSpawn(int turn)
    {
        return (int)Math.Round((turn * 0.35)*0.75);
    }

    public override int GetSnipperToSpawn(int turn)
    {
        return (int)Math.Round(turn *0.1);
    }
}
