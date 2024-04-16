using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/FourthQuadraSpawnerMath")]
public class FourthQuadraSpawnerMath :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        if (turn <= 7)
        {
            return (int)(Math.Ceiling(turn *0.14));
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
            return (int)(Math.Round((turn *0.3) /2.4 ));
        }
        else
        {
            return 0;
        }
    }

    public override int GetDoggoToSpawn(int turn)
    {
        if (turn <= 7)
        {
            return (int)(Math.Round((turn * 0.05)));
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
