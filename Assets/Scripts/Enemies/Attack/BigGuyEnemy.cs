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

        public override AttackingInfo ChoseToAttack()
        {
            Vector2Int positionInFront = cell.position + new Vector2Int(0, -1);
            Cell updatedCell;
            try
            {
                updatedCell = TilingGrid.grid.GetCell(positionInFront);
            }
            catch (ArgumentException)
            {
                return new AttackingInfo();
            }
            if (updatedCell.HasTopOfCellOfType(TypeTopOfCell.Obstacle))
            {
                Obstacle obstacle = updatedCell.GetObstacle();
                int remainingHP = obstacle.Damage(AttackDamage);
                bool shouldKill = remainingHP <= 0;
                if (shouldKill)
                {
                    obstacle.CleanUp();
                }
                return new AttackingInfo()
                {
                    shouldKill = shouldKill,
                    hasAttacked = true,
                    isTower = false,
                    toKill = obstacle.ToGameObject(),
                };
            }

            if (updatedCell.HasNonWalkableBuilding())
            {
                var tower = updatedCell.GetTower();
                int remainingHP = tower.Damage(AttackDamage);
                bool shouldKill = remainingHP <= 0;
                if (shouldKill)
                {
                    tower.Clean();
                }
                return new AttackingInfo()
                { 
                    shouldKill = shouldKill,
                    hasAttacked = true,
                    isTower = true,
                    toKill = tower.ToGameObject(),
                };
            }

            return new AttackingInfo();
        }
    }
}