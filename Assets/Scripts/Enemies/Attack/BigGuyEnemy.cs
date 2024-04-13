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
        public static int BigGuyMoveRatio;

        private int _attack = BigGuyAttack;
        private int _health = BigGuyHealth;
        private int _moveRatio = BigGuyMoveRatio; 
        public override int MoveRatio { get => _moveRatio; set => _moveRatio = value; }
        public override int Health { get => _health; set => _health =  value ; }
        
        public override int AttackDamage { get => _attack; set => _attack = value; }

        public BigGuyEnemy()
        {
            ennemyType = EnnemyType.BigGuy;
        }

        public override (bool, bool) ChoseToAttack()
        {
            Vector2Int positionInFront = cell.position + new Vector2Int(0, -1);
            Cell updatedCell;
            try
            {
                updatedCell = TilingGrid.grid.GetCell(positionInFront);
            }
            catch (ArgumentException)
            {
                return (false,false);
            }
            if (updatedCell.HasTopOfCellOfType(TypeTopOfCell.Obstacle))
            {
                return (true, Attack(updatedCell.GetObstacle()));
            }

            if (updatedCell.HasTopOfCellOfType(TypeTopOfCell.Obstacle))
            {
                return (true, Attack(updatedCell.GetTower()));
            }

            return (false, false);
        }
    }
}