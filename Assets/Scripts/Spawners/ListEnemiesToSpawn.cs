using UnityEngine;

namespace Spawners
{
    [CreateAssetMenu()]
    public class ListEnemiesToSpawnSO : ScriptableObject
    {
        [SerializeField] public GameObject Doggo; 
        [SerializeField] public GameObject Merde; 
        [SerializeField] public GameObject BigGuy; 
        [SerializeField] public GameObject Sniper; 
        
    }
}