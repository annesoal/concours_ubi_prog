using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/ThirdQuadraSpawnerMath")]
public class ThirdQuadraSpawnerMath :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn <= 7)
        {
            return (int)(Math.Ceiling((turn + 1) * 0.1));
        }
        else
        {
            return 0;
        }

    }

    public override int GetBigGuyToSpawn(int turn)
    {
        if (turn <= 7)
        {
            return (int)(Math.Max(Math.Round((turn - 1)*0.15),0));
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
        if (turn <= 7)
        {
            return (int)(Math.Ceiling(((turn * 0.15))/2));
        }
        else
        {
            return 0;
        }
    }
}
