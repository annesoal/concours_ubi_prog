using System;
using System.Collections;
using Grid;
using Grid.Interface;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Random = System.Random;

namespace Enemies
{
    public class BasicEnemy : Enemy
    {
        private Random _rand = new();
        protected float timeToMove = 1.0f;
        
        public BasicEnemy()
        {
            ennemyType = EnnemyType.PetiteMerde;
        }
        
        
        public override IEnumerator Move(int energy)
        {
            if (!IsServer)
            {
                yield break;
            }

            if (!IsTimeToMove(energy))
            {
                hasFinishedToMove = true;
                yield break;
            }

            hasFinishedToMove = false;
            if (!TryMoveOnNextCell())
            {
                hasPath = false;
                if (!MoveSides())
                {
                    //TODO 
                    transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
            }

            yield return new WaitUntil(hasFinishedMovingAnimation);
            hasFinishedToMove = true;
            EmitOnAnyEnemyMoved();
        }

        public override bool PathfindingInvalidCell(Cell cellToCheck)
        {
            return cellToCheck.HasTopOfCellOfType(TypeTopOfCell.Obstacle) ||
                   cellToCheck.HasNonWalkableBuilding();
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
            path.RemoveAt(0);
            if (IsValidCell(nextCell))
            {
                cell = nextCell;
                StartCoroutine(MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position)));
                return true;
            }

            return false;
        }


        //Commence a aller vers la droite ou la gauche aleatoirement
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


        //Essayer de bouger vers direction
        private bool TryMoveOnNextCell(Vector2Int direction)
        {
            bool isLeft = direction == _gauche2d;
            Vector2Int nextPosition = new Vector2Int(cell.position.x + direction.x, cell.position.y + 1);
            Cell nextCell = TilingGrid.grid.GetCell(nextPosition);

            // tout de suite changer l'orientation ??
            if (IsValidCell(nextCell))
            {
                cell = TilingGrid.grid.GetCell(nextPosition);

                StartCoroutine(
                    MoveEnemy(
                        TilingGrid.GridPositionToLocal(nextPosition)));
                
                return true;
            }

            return false;
        }

        private IEnumerator TurnEnemy(Vector2Int direction)
        {
            if (!IsServer) yield break;
            float currentTime = 0.0f;
            Vector3 origin = transform.position;
            var turnPosition = SetTurnDegreePosition(direction, origin);


            while (timeToMove > currentTime)
            {
                transform.position = Vector3.Lerp(
                    origin, turnPosition, currentTime / timeToMove);
                currentTime += Time.deltaTime;
                yield return null;
            }

            animator.SetBool("TurnLeft", true);
        }


        private Vector3 SetTurnDegreePosition(Vector2Int direction, Vector3 origin)
        {
            Vector3 turnPosition = origin;

            if (direction == _gauche2d)
            {
                turnPosition.y = 90;
            }
            else
            {
                turnPosition.y = -90;
            }

            return turnPosition;
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
                    origin, direction, currentTime / timeToMove);
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

        private bool IsValidCell(Cell toCheck)
        {
            PathfindingInvalidCell(toCheck);
            bool isValidBlockType = (toCheck.type & BlockType.EnemyWalkable) > 0;
            bool hasNoEnemy = !TilingGrid.grid.HasTopOfCellOfType(toCheck, TypeTopOfCell.Enemy);

            return isValidBlockType && hasNoEnemy && !PathfindingInvalidCell(toCheck);
        }
    }
}