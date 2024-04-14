using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Building.Traps
{
    public struct TrapPlayInfo
    {
        public bool isTrigger;
        public List<EnemyAffectedInfo> enemiesAffectedInfo;
    }

    public struct EnemyAffectedInfo
    {
        public Enemy enemy;
        public bool shouldKill;

    }
}