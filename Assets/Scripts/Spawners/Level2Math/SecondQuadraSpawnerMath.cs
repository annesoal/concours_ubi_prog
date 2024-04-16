using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/SecondQuadraSpawnerMath")]
public class SecondQuadraSpawnerMath :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn <= 7)
        {
            return (int)(Math.Ceiling((turn * 0.55) / 4));
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
        if (turn <= 7)
        {
            return (int)(Math.Round(((turn * 0.35)*0.75)/2));
        }
        else
        {
            return 0;
        }
    }

    public override int GetSnipperToSpawn(int turn)
    {
        if (turn <= 7)
        {
            return (int)(Math.Round((turn *0.1)/2));
        }
        else
        {
            return 0;
        }
    }
}
