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
    public class AttackingEnemy : BasicEnemy 
    {
        private Random _rand = new();
        private int enemyDamage = 1;
        private int attackRate;
        private int radiusAttack = 1;
        protected float timeToAttack = 0.5f;
        
        /**
         * Bouge aleatoirement selon les cells autour de l'ennemi.
         * - Attaquer un obstacle ou une tower
         * - Avancer (et eviter ?)
         */
        public override IEnumerator Move(int energy)
        {
            {
                if (!IsServer) yield break;
                
                if (!IsTimeToMove(energy))
                {
                    hasFinishedToMove = true;
                    yield break;
                }
                
                hasFinishedToMove = false;
                if (!ChoseToAttack())
                {
                    if (!TryMoveOnNextCell())
                    {
                        hasPath = false;
                        if (!MoveSides())
                        {
                            hasFinishedMoveAnimation = true;
                        }
                    }
                }
            }
            yield return new WaitUntil(hasFinishedMovingAnimation);
            hasFinishedToMove = true;
            EmitOnAnyEnemyMoved();
        }

        public bool ChoseToAttack()
        {
            if (path == null || path.Count == 0)
                return true;
            
            List<Cell> cellsInRadius =
                TilingGrid.grid.GetCellsInRadius(TilingGrid.LocalToGridPosition(transform.position), radiusAttack);
            return (ChoseAttack(cellsInRadius));
         
        }

        private bool ChoseAttack(List<Cell> cellsInRadius)
        {
            foreach (var aCell in cellsInRadius)
            {
                if (TilingGrid.grid.HasTopOfCellOfType(aCell, TypeTopOfCell.Building) &&
                    canAttack())
                {
                    Attack(aCell.GetTower());
                    hasPath = false;
                    return true;
                }
            }

            return false;
        }
        
        // Choisit d'attaquer aleatoirement
        private bool canAttack()
        {
            return (_rand.NextDouble() > 1 - attackRate);
        }
        
        private void Attack(BaseTower toAttack)
        {
            toAttack.Damage(enemyDamage);
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

        private bool IsTimeToMove(int energy)
        {
            return energy % ratioMovement == 0;
        }
        
        private bool hasFinishedMovingAnimation()
        {
            return hasFinishedMoveAnimation;
        }

        protected override bool IsValidCell(Cell toCheck)
        {
            Cell updatedCell = TilingGrid.grid.GetCell(toCheck.position);
            bool hasObstacleOnTop = updatedCell.HasTopOfCellOfType(TypeTopOfCell.Obstacle);
            return base.IsValidCell(toCheck) && !hasObstacleOnTop;
        }
    }
}
