using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Building.Towers
{
    public struct TowerPlayInfo
    {
        public bool hasFired;
        public List<EnemyInfoToShoot> listEnemiesToShoot;
    }

    public struct EnemyInfoToShoot
    {
        public Vector3 position;
        public Enemy enemy;
        public bool shouldKill;
    }
}