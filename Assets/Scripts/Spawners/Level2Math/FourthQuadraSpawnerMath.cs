using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/FourthQuadraSpawnerMath")]
public class FourthQuadraSpawnerMath :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        return (int)(Math.Ceiling(turn *0.14));
    }

    public override int GetBigGuyToSpawn(int turn)
    {
        return (int)(Math.Round((turn *0.3) /2.4 ));
    }

    public override int GetDoggoToSpawn(int turn)
    {
        return (int)(Math.Round((turn * 0.05)));
    }

    public override int GetSnipperToSpawn(int turn)
    {
        return 0;
    }
}
