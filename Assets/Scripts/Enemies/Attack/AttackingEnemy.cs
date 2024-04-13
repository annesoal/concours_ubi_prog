using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.Basic;
using Grid;
using Grid.Interface;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

namespace Enemies
{
    public abstract class AttackingEnemy : BasicEnemy
    {
        public abstract int AttackDamage { get; set; }

        protected override (bool hasReachedEnd, bool moved, bool attacked, bool shouldKill, Vector3 destination) BackendMove()
        {
             Assert.IsTrue(IsServer);
                        
            if (HasReachedTheEnd())
            {
                Debug.Log("reched end at " + transform.position + " cell pos "  + cell.position);
                return (true, false, false,false, Vector3.zero);
            }

            if (!IsTimeToMove() || isStupefiedState > 0)
            {
                return (false, false, false,false, Vector3.zero);
            }

            var attackInfo = ChoseToAttack();
            if ( attackInfo.Item1)
            {
                return (false, false, true,attackInfo.Item2, Vector3.zero);
            }
            
            if (!TryMoveOnNextCell())
            {
                hasPath = false;
                if (!MoveSides())
                {
                    return (false, false, false,false, Vector3.zero);
                }
                else
                {
                    return (false, true, false,false, TilingGrid.GridPositionToLocal(cell.position));
                }
            }
            return (false, true, false,false, TilingGrid.GridPositionToLocal(cell.position));
        }

        public abstract (bool, bool) ChoseToAttack();
        public (bool hasAttacked, bool shouldKill) ChoseAttack(List<Cell> cellsInRadius)
        {
            foreach (var aCell in cellsInRadius)
            {
                if (TowerIsAtRange(aCell) &&
                    canAttack())
                {
                    hasPath = false;
                    return (true, Attack(aCell.GetTower()));
                }
            }
            return (false, false);
        }

        private bool TowerIsAtRange(Cell aCell)
        {
            // non walkable building are towers or obstacle.
            return TilingGrid.grid.HasTopOfCellOfType(aCell, TypeTopOfCell.Building) &&
                   cell.HasNonWalkableBuilding();
        }
        
        private bool canAttack()
        {
            return true;
        }
        
        protected bool Attack(BaseTower toAttack)
        {
            int remainingHP = toAttack.Damage(AttackDamage);
            return remainingHP <= 0;
        }
        
        protected bool Attack(Obstacle toAttack)
        {
            return toAttack.Damage(AttackDamage) <= 0;
        }

        private IEnumerator AttackAnimation()
        {
            if (!IsServer) yield break;
            hasFinishedMoveAnimation = false;
            animator.SetBool("Attack", true);
            float currentTime = 0.0f;

            //TODO time to attack? et regarder avec anim tour
            while (timeToMove > currentTime)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }
            
            animator.SetBool("Attack", false);
            hasFinishedMoveAnimation = true;
        }

        // Peut detruire obstacle et tower, tous les cells avec obstacles `solides` sont valides 
        public override bool PathfindingInvalidCell(Cell cellToCheck)
        {
            return false;
        }

        protected override bool IsValidCell(Cell toCheck)
        {
            Cell updatedCell = TilingGrid.grid.GetCell(toCheck.position);
            bool hasObstacleOnTop = updatedCell.HasTopOfCellOfType(TypeTopOfCell.Obstacle);
            return base.IsValidCell(toCheck) && !hasObstacleOnTop;
        }


        public override void MoveCorroutine(EnemyChoicesInfo infos)
        {
            if (infos.hasAttacked)
            {
                StartCoroutine(AttackAnimation());
                return;
            }
            else
            {
                base.MoveCorroutine(infos);
            }
        }
    }
}
