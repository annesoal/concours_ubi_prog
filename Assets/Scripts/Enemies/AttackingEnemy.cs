using System;
using System.Collections;
using System.Collections.Generic;
using Ennemies;
using Grid;
using Grid.Interface;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
using Interfaces;

namespace Enemies
{
    public class AttackingEnemy : Enemy
    {
        private Random _rand = new();
        [SerializeField] private int enemyDomage = 1;
        [SerializeField] private int attackRate;
        [SerializeField] private int radiusAttack = 1;
        protected float timeToMove = 1.0f;
        
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
                        if (!MoveSides())
                        {
                            //TODO
                            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            
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
            Cell nextCell = path[0];
            path.RemoveAt(0);

            
            List<Cell> cellsInRadius =
                TilingGrid.grid.GetCellsInRadius(TilingGrid.LocalToGridPosition(transform.position), radiusAttack);
            if (ChoseAttackObstacle(cellsInRadius)) return true;

            // TODO pour tower
            return false;
        }

        private bool ChoseAttackObstacle(List<Cell> cellsInRadius)
        {
            foreach (var aCell in cellsInRadius)
            {
                if (TilingGrid.grid.HasTopOfCellOfType(aCell, TypeTopOfCell.Building) &&
                    IsAttacking(aCell.GetTower()))
                {
                    hasPath = false;
                    return true;
                }
            }

            return false;
        }


        // Choisit d'attaquer selon aleatoirement
        private bool IsAttacking(BaseTower toAttack)
        {
            if (_rand.NextDouble() > 1 - attackRate)
            {
                toAttack.Damage(enemyDomage);
                StartCoroutine(AttackAnimation(toAttack));
                
                return true;
            }

            return false;
        }

        private IEnumerator AttackAnimation(BaseTower toAttack)
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

        // Essaie de bouger vers l'avant
        private bool TryMoveOnNextCell()
        {
            if (path == null || path.Count == 0)
                return true;

            Cell nextCell = path[0];
            if (IsValidCell(nextCell))
            {
                cell = nextCell;
                StartCoroutine(MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position)));
                return true;
            }

            return false;
        }

        private bool MoveSides()
        {
            if (_rand.NextDouble() < 0.5)
            {
                if (!TryMoveOnNextCell(_gauche2d))
                {
                    return TryMoveOnNextCell(_droite2d);
                }
            }
            else
            {
                if (!TryMoveOnNextCell(_droite2d))
                {
                    return TryMoveOnNextCell(_gauche2d);
                }
            }

            return false;
        }

        private bool TryMoveOnNextCell(Vector2Int direction)
        {
            Vector2Int nextPosition = new Vector2Int(cell.position.x + direction.x, cell.position.y + direction.y);
            Cell nextCell = TilingGrid.grid.GetCell(nextPosition);

            if (IsValidCell(nextCell))
            {
                cell = TilingGrid.grid.GetCell(nextPosition);
                MoveEnemy(TilingGrid.GridPositionToLocal(nextPosition));
                return true;
            }

            return false;
        }

        /*
         * Bouge l'ennemi
         */
        private IEnumerator MoveEnemy(Vector3 direction)
        {
            if (!IsServer) yield break;
            hasFinishedMoveAnimation = false;
            animator.SetBool("Move", true);
            TilingGrid.grid.RemoveObjectFromCurrentCell(this.gameObject);
            float currentTime = 0.0f;
            Vector3 origin = transform.position;
            while (timeToMove > currentTime)
            {
                transform.position = Vector3.Lerp(
                    origin, direction, currentTime/timeToMove);
                currentTime += Time.deltaTime;
                yield return null;
            } 
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, direction);
            animator.SetBool("Move", false);
            hasFinishedMoveAnimation = true;
        }
        
        private bool hasFinishedMovingAnimation()
        {
            return hasFinishedMoveAnimation;
        }

        
        protected override bool TryStepBackward()
        {
            Vector2Int nextPosition = new Vector2Int(cell.position.x, cell.position.y + 1);
            Cell nextCell = TilingGrid.grid.GetCell(nextPosition);

            if (IsValidCell(nextCell))
            {
                cell = TilingGrid.grid.GetCell(nextPosition);
                StartCoroutine(
                    MoveEnemy(
                        TilingGrid.GridPositionToLocal(nextPosition)));

                return true;
            }
            // TODO REVENIR SUR PLACE SI PEUT PAS RECULER ??
            return false;
        }
        
        
        private bool IsValidCell(Cell cell)
        {
            bool isValidBlockType = (cell.type & BlockType.EnemyWalkable) > 0;
            bool hasNoEnemy = !TilingGrid.grid.HasTopOfCellOfType(cell, TypeTopOfCell.Enemy);
            bool hasNoObstacle = !TilingGrid.grid.HasTopOfCellOfType(cell, TypeTopOfCell.Obstacle);
            return isValidBlockType && hasNoEnemy && hasNoObstacle;
        }
    }
}