using System;
using System.Collections;
using System.Collections.Generic;
using Spawners;
using UnityEngine;

public class FirstQuadraSpawnerMath :  IMathSpawn
{
    public int GetNumberMerdeToSpawn(int turn)
    {
        return 0;
    }

    public int GetBigGuyToSpawn(int turn)
    {
        return (int) Math.Max(Math.Ceiling((turn - 1) * 0.1*0.75), 0);
    }

    public int GetDoggoToSpawn(int turn)
    {
        return (int)Math.Round((turn * 0.5)*0.75);
    }

    public int GetSnipperToSpawn(int turn)
    {
        return (int)Math.Round((turn * 0.2)*0.75);
    }
}
