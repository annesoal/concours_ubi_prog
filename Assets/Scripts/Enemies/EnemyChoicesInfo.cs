using Ennemies;
using UnityEngine;

namespace Enemies
{
    public struct EnemyChoicesInfo
    {
        
           public bool hasMoved;
           public bool hasReachedEnd;
           public AttackingInfo attack;
           public Vector3 destination;    
    }

    public struct AttackingInfo
    {
        public bool hasAttacked;
        public bool shouldKill;
        public bool isTower;
        public GameObject toKill;
    }
}