using System.Collections;
using Grid;
using Grid.Interface;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;
using Random = System.Random;

namespace Enemies.Basic
{
    public abstract class BasicEnemy : Enemy
    {
        private Random _rand = new();
        protected float timeToMove = 0.4f;

        public BasicEnemy()
        {
            ennemyType = EnnemyType.PetiteMerde;
        }
        protected override (bool hasReachedEnd, bool moved, bool attacked, bool shouldKill, Vector3 destination) BackendMove()
        {
            Assert.IsTrue(IsServer);
            if (HasReachedTheEnd())
            {
                return (true, false, false, false, Vector3.zero);
            }
            if (!IsTimeToMove() || isStupefiedState > 0)
            {
                return (false, false, false,false, Vector3.zero);
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

        public bool HasReachedTheEnd()
        {
            Cell updateCell = TilingGrid.grid.GetCell(cell.position);
            return updateCell.Has(BlockType.EnemyDestination);
        }

        public override bool PathfindingInvalidCell(Cell cellToCheck)
        {
            return cellToCheck.HasTopOfCellOfType(TypeTopOfCell.Obstacle) ||
                   cellToCheck.HasNonWalkableBuilding();
        }

        protected bool IsTimeToMove()
        {
            if (_actionTimer-- != 0) return false;
            
            _actionTimer = MoveRatio;
            return true;
        }


        // Essaie de bouger vers l'avant
        protected bool TryMoveOnNextCell()
        {
            if (path == null || path.Count == 0)
            {
                return true;
            }
            Cell nextCell = path[0];
            path.RemoveAt(0);
            if (IsValidCell(nextCell))
            {
                TilingGrid.grid.RemoveObjectFromCurrentCell(this.gameObject);
                cell = nextCell;
                
                TilingGrid.grid.AddObjectToCellAtPositionInit(gameObject, cell.position);
                return true;
            }
            return false;
        }


        //Commence a aller vers la droite ou la gauche aleatoirement
        protected bool MoveSides()
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

            return true;
        }

        protected override bool TryStepBackward()
        {
            Vector2Int nextPosition = new Vector2Int(cell.position.x, cell.position.y);
            Cell nextCell = TilingGrid.grid.GetCell(nextPosition);

            if (IsValidCell(nextCell))
            {
                TilingGrid.grid.RemoveObjectFromCurrentCell(this.gameObject);
                cell = TilingGrid.grid.GetCell(nextPosition);
                TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, cell.position);
                return true;
            }

            // TODO REVENIR SUR PLACE SI PEUT PAS RECULER ??
            return false;
        }


        //Essayer de bouger vers direction
        private bool TryMoveOnNextCell(Vector2Int direction)
        {
            Vector2Int nextPosition = new Vector2Int(cell.position.x + direction.x, cell.position.y + direction.y);
            Cell nextCell = TilingGrid.grid.GetCell(nextPosition);
            
            if (IsValidCell(nextCell))
            {
                Debug.LogWarning("prev cell : " + cell.position);
                TilingGrid.grid.RemoveObjectFromCurrentCell(this.gameObject);
                cell = nextCell;
                
                Debug.LogWarning("new cell : " + cell.position);
                TilingGrid.grid.AddObjectToCellAtPositionInit(gameObject, cell.position);
                return true;
            }
            return false;
        }

        protected override IEnumerator RotateThenMove(Vector3 direction)
        {
            RotationAnimation rotationAnimation = new RotationAnimation();
            StartCoroutine(rotationAnimation.TurnObjectTo(this.gameObject, direction));
            yield return new WaitUntil(rotationAnimation.HasMoved);
            StartCoroutine(MoveEnemy(direction));
            yield return new WaitUntil(hasFinishedMovingAnimation);
        }


        /*
         * Bouge l'ennemi
         */
        protected virtual IEnumerator MoveEnemy(Vector3 direction)
        {
            if (!IsServer) yield break;
            hasFinishedMoveAnimation = false;
            animator.SetBool("Move", true);
            float currentTime = 0.0f;
            Vector3 origin = transform.position;
            while (timeToMove > currentTime)
            {
                transform.position = Vector3.Lerp(
                    origin, direction, currentTime / timeToMove);
                currentTime += Time.deltaTime;
                yield return null;
            }

            animator.SetBool("Move", false);
            hasFinishedMoveAnimation = true;
        }

        protected bool hasFinishedMovingAnimation()
        {
            return hasFinishedMoveAnimation;
        }

         protected virtual bool IsValidCell(Cell toCheck)
        {
            Cell updatedCell = TilingGrid.grid.GetCell(toCheck.position);
            bool isValidBlockType = (updatedCell.type & BlockType.EnemyWalkable) > 0;
            bool hasNoEnemy = !TilingGrid.grid.HasTopOfCellOfType(updatedCell, TypeTopOfCell.Enemy);
            return isValidBlockType && hasNoEnemy && !PathfindingInvalidCell(updatedCell);
        }
    }
}
