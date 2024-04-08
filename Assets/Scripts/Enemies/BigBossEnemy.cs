using Unity.Netcode;
using UnityEngine;

namespace Enemies
{
    public class BigBossEnemy : NetworkBehaviour
    {
       [SerializeField] private Animator animator; 
    }
}