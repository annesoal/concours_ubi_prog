using UnityEngine;

namespace Enemies
{
    public struct EnemyChoicesInfo
    {
        
           public bool hasMoved;
           public bool hasReachedEnd;
           public bool hasAttacked;
           public bool shouldKill;
           public Vector3 destination;    
    }
}