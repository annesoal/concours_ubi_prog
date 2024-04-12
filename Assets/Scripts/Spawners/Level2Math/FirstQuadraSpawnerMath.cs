using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/FirstQuadraSpawnerMath")]
public class FirstQuadraSpawnerMath :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        return 0;
    }

    public override int GetBigGuyToSpawn(int turn)
    {
        return (int) Math.Max(Math.Ceiling((turn - 1) * 0.1*0.75), 0);
    }

    public override int GetDoggoToSpawn(int turn)
    {
        return (int)Math.Round((turn * 0.5)*0.75);
    }

    public override int GetSnipperToSpawn(int turn)
    {
        return (int)Math.Ceiling((turn * 0.2)*0.75);
    }
}
