using System;
using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;

namespace Enemies
{
    public class BasicEnemy : Enemy
    {
        private Random _rand = new();

        public BasicEnemy()
        {
            ennemyType = EnnemyType.PetiteMerde;
        }

        protected override void Initialize()
        {
            AddInGame(this.gameObject);
        }


        public override void Move(int energy)
        {
            if (!IsServer) return;

        
            if (!IsTimeToMove(energy)) return;
            if (!TryMoveOnNextCell())
            {
                hasPath = false;
                if (!MoveSides())
                {
                    //TODO 
                    transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
            }

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
                MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position));
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

        //Essayer de bouger vers direction
        private bool TryMoveOnNextCell(Vector2Int direction)
        {
            Vector2Int nextPosition = new Vector2Int(cell.position.x + direction.x, cell.position.y + direction.y);
            Cell nextCell = TilingGrid.grid.GetCell(nextPosition);

            if (IsValidCell(nextCell))
            {
                cell = TilingGrid.grid.GetCell(nextPosition);
                
                if(direction==_gauche2d) 
                    animator.SetBool("IsTurnLeft", true);
                if(direction==_droite2d)
                    animator.SetBool("IsTurnRight", true);
              
                MoveEnemy(TilingGrid.GridPositionToLocal(nextPosition));

                if(direction==_gauche2d) 
                    animator.SetBool("IsTurnLeft", false);
                if(direction==_droite2d)
                    animator.SetBool("IsTurnRight", false);
                 
               
                return true;
            }
            
            
            return false;
        }

        /*
         * Bouge l'ennemi
         */
        private void MoveEnemy(Vector3 direction)
        {
            if (!IsServer) return;
            animator.SetBool("IsMoving", true);
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(this.gameObject, direction);
            animator.SetBool("IsMoving", false);
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