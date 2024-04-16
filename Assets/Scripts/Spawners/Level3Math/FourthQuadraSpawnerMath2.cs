using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName ="Math/FourthQuadraSpawnerMath2")]
public class FourthQuadraSpawnerMath2 :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn <= 10)
        {
            return (int) Math.Ceiling(turn *0.28);
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
            return (int) Math.Round(turn *0.1);
        }
        else
        {
            return 0;
        }
    }

    public override int GetDoggoToSpawn(int turn)
    {
        if (turn <= 10)
        {
            return (int)Math.Round((turn * 0.15));
        }
        else
        {
            return 0;
        }
    }

    public override int GetSnipperToSpawn(int turn)
    {
        return 0;
    }
}
