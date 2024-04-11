using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()]
public class PairInputPathAndSpriteSO : ScriptableObject
{
    public List<PairInputPathAndSprite> pairsList;

    [Serializable]
    public struct PairInputPathAndSprite
    {
        public string path;
        public Sprite sprite;
    }
}
