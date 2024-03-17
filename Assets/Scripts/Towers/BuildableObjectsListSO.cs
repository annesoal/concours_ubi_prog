using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()]
public class BuildableObjectsListSO : ScriptableObject
{
    [SerializeField] public List<BuildableObjectSO> allTowersList;
}
