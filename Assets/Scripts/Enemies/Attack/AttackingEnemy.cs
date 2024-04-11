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
            //Debug.Log("Dans move attacking enemy");

            if (!IsServer)
            {
                //Debug.Log("Dans early return IsServer");
                yield break;
            }
                
            if (!IsTimeToMove(energy))
            {
                //Debug.Log("Early return IsTimeToMove dans attacking enemy");
                hasFinishedMoveAnimation = true;
                hasFinishedToMove = true;
                yield break;
            }

            if (isStupefiedState)
            {
                //Debug.Log("Early return isStupefiedState dans attacking enemy");
                hasFinishedMoveAnimation = true;
                hasFinishedToMove = true;
                yield break;
            }
                
            hasFinishedToMove = false;
            if (!ChoseToAttack())
            {
                //Debug.Log("Apres chose to attack");
                if (!TryMoveOnNextCell())
                {
                    //Debug.Log("Apres try move on next cell");
                    hasPath = false;
                    if (!MoveSides())
                    {
                        //Debug.Log("Apres move sides");
                        hasFinishedMoveAnimation = true;
                    }
                }
            }
            else
            {
                hasFinishedMoveAnimation = true;
            }
            
            yield return new WaitUntil(hasFinishedMovingAnimation);
            hasFinishedToMove = true;
            EmitOnAnyEnemyMoved();
        }

        public virtual bool ChoseToAttack()
        {
            if (path == null || path.Count == 0)
            {
                Debug.Log("Dans le early return de chose to attack");
                hasFinishedMoveAnimation = true;
                return true;
            }
            
            List<Cell> cellsInRadius =
                TilingGrid.grid.GetCellsInRadius(TilingGrid.LocalToGridPosition(transform.position), radiusAttack);
            return (ChoseAttack(cellsInRadius));
        }

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
            return (_rand.NextDouble() > 1 - attackRate);
        }
        
        protected void Attack(BaseTower toAttack)
        {
            toAttack.Damage(enemyDamage);
            StartCoroutine(AttackAnimation());
        }
        
        protected void Attack(Obstacle toAttack)
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


        protected override bool IsValidCell(Cell toCheck)
        {
            Cell updatedCell = TilingGrid.grid.GetCell(toCheck.position);
            bool hasObstacleOnTop = updatedCell.HasTopOfCellOfType(TypeTopOfCell.Obstacle);
            return base.IsValidCell(toCheck) && !hasObstacleOnTop;
        }
    }
}
