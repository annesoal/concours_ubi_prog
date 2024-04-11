using System.Collections;
using System.Collections.Generic;
using Amulets;
using UnityEngine;


[CreateAssetMenu()]
public class NextLevelDataSO : ScriptableObject
{
    public List<AmuletSO> AmuletChoiceAtEnd;

    public Loader.Scene nextLevelScene;
}
