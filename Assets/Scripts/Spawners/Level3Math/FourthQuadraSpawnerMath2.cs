using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName ="Math/FourthQuadraSpawnerMath2")]
public class FourthQuadraSpawnerMath2 :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        return (int) Math.Ceiling(turn *0.28);
    }

    public override int GetBigGuyToSpawn(int turn)
    {
        return (int) Math.Round(turn *0.1);
    }

    public override int GetDoggoToSpawn(int turn)
    {
        return (int)Math.Round((turn * 0.15));
    }

    public override int GetSnipperToSpawn(int turn)
    {
        return 0;
    }
}
