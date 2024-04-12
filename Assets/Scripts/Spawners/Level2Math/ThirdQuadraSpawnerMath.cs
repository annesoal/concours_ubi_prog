using System;
using Spawners;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/ThirdQuadraSpawnerMath")]
public class ThirdQuadraSpawnerMath :  MathSpawnSO
{
    public override int GetNumberMerdeToSpawn(int turn)
    {
        return (int) Math.Ceiling((turn + 1) * 0.2);
        
    }

    public override int GetBigGuyToSpawn(int turn)
    {
        return (int)Math.Max(Math.Round((turn - 1)*0.3),0);
    }

    public override int GetDoggoToSpawn(int turn)
    {
        return 0;
    }

    public override int GetSnipperToSpawn(int turn)
    {
        return (int)Math.Ceiling((turn * 0.15));
    }
}
