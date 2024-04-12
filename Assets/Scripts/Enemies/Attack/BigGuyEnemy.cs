using System;
using Grid;
using Grid.Interface;
using UnityEngine;

namespace Enemies.Attack
{
    public class BigGuyEnemy : AttackingEnemy
    {
        public static int BigGuyHealth;
        public static int BigGuyAttack;

        private int _attack = BigGuyAttack;
        private int _health = BigGuyHealth;
        public override int Health { get => _health; set => _health =  value ; }
        
        public override int AttackDamage { get => _attack; set => _attack = value; }

        public BigGuyEnemy()
        {
            ennemyType = EnnemyType.BigGuy;
        }

        public override bool ChoseToAttack()
        {
            if (path == null || path.Count == 0)
                return true;
            Vector2Int positionInFront = cell.position + new Vector2Int(0, -1);
            Cell updatedCell;
            try
            {
                updatedCell = TilingGrid.grid.GetCell(positionInFront);
            }
            catch (ArgumentException)
            {
                return false;
            }
            if (updatedCell.HasTopOfCellOfType(TypeTopOfCell.Obstacle))
            {
                base.Attack(updatedCell.GetObstacle());
                return true;
            }

            if (updatedCell.HasTopOfCellOfType(TypeTopOfCell.Obstacle))
            {
                base.Attack(updatedCell.GetTower());
                return true;
            }

            return false;
        }
    }
}