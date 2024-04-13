using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.Basic;
using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;

namespace Enemies
{
    public abstract class AttackingEnemy : BasicEnemy
    {
        public abstract int AttackDamage { get; set; }

        protected override (bool hasReachedEnd, bool moved, bool attacked, Vector3 destination) BackendMove()
        {
            if (ChoseToAttack())
            {
                return (false, false, true, new Vector3());
            }

            return base.BackendMove();
        }

        public abstract bool ChoseToAttack();
        public bool ChoseAttack(List<Cell> cellsInRadius)
        {
            Debug.Log("Dans chose to attack avec cell in radius");
            foreach (var aCell in cellsInRadius)
            {
                if (TowerIsAtRange(aCell) &&
                    canAttack())
                {
                    Attack(aCell.GetTower());
                    hasPath = false;
                    return true;
                }
            }
            return false;
        }

        private bool TowerIsAtRange(Cell aCell)
        {
            // non walkable building are towers or obstacle.
            return TilingGrid.grid.HasTopOfCellOfType(aCell, TypeTopOfCell.Building) &&
                   cell.HasNonWalkableBuilding();
        }
        
        // Choisit d'attaquer aleatoirement
        private bool canAttack()
        {
            return true;
        }
        
        protected void Attack(BaseTower toAttack)
        {
            toAttack.Damage(AttackDamage);
            StartCoroutine(AttackAnimation());
        }
        
        protected void Attack(Obstacle toAttack)
        {
            toAttack.Damage(AttackDamage);
            StartCoroutine(AttackAnimation());
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
            }
            else
            {
                base.MoveCorroutine(infos);
            }
        }
    }
}
