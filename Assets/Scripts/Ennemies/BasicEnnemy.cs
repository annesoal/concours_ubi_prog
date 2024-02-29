using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class BasicEnnemy : Ennemy
    {
        [SerializeField] private EnnemyType ennemyType = EnnemyType.Basic;

        public override void Move()
        {
            base.Move();
        }

        public override void Corrupt()
        {
            base.Corrupt();
        }
    }
}