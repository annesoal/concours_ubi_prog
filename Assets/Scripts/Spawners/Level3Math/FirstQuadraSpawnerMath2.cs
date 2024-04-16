using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/FirstQuadraSpawnerMath2")]
public class FirstQuadraSpawnerMath2 :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        return 0;
    }

    public override int GetBigGuyToSpawn(int turn)
    {
        if (turn <= 10)
        {
            return (int) Math.Max(Math.Ceiling((turn - 1) * 0.075), 0);
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
            return (int)Math.Floor((turn * 0.4)*0.6);
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
            return (int)Math.Ceiling((turn * 0.2)*0.75);
        }
        else
        {
            return 0;
        }
    }
}
