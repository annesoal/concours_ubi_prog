using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()]
public class TowersListSO : ScriptableObject
{
    [SerializeField] public List<BuildableObjectSO> allTowersList;
}
