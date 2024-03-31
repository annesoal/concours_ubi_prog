using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSelectSO : ScriptableObject
{
    public string levelName;
    public Loader.Scene levelScene;
}
