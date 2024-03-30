using System;
using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;

namespace Enemies
{
    public sealed class BasicEnemy : Enemy
    {
        private Random _rand = new();

        public BasicEnemy()
        {
            ennemyType = EnnemyType.Basic;
            health = 1;
        }


        protected override void Initialize()
        {
            AddInGame(this.gameObject);
        }


        /**
         * Deplace un ennemi d'un block :
         *      Vers l'avant si aucun obstacle
         *      Gauche ou droite si un obstacle
         */
        public override void Move(int energy)
        {
            {
                if (!IsServer) return;
                if (!IsTimeToMove(energy)) return;

                if (!TryMoveOnNextCell())
                {
                    if (MoveSides())
                    {
                        hasPath = false;
                        
                    }
                    else
                    {
                        Debug.Log("ERROR derniere position BASIC: " +cell.position);
                        Debug.Log("ERROR path position " + path[0].position);
                        throw new Exception("moveside did not work, case not implemented yet !");
                    }
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

        //Commence a aller vers la droite ou la gauche aleatoirement
        private bool MoveSides()
        {
            if (_rand.NextDouble() < 0.5)
            {
                Debug.Log("1- Gauche ");
                if (!TryMoveOnNextCell(_gauche2d))
                {
                    Debug.Log("2- Droite ");
                    return TryMoveOnNextCell(_droite2d);
                }
            }
            else
            {
                Debug.Log("1- Droite ");
                if (!TryMoveOnNextCell(_droite2d))
                {
                    Debug.Log("2- Gauche ");
                    return TryMoveOnNextCell(_gauche2d);
                }
            }

            return false;
        }

        // Essaie de bouger vers l'avant
        private bool TryMoveOnNextCell()
        {
            if (path == null || path.Count == 0)
                return true;

            Cell nextCell = path[0];
            path.RemoveAt(0);
            Debug.Log("BASIC PATH next cell position" + nextCell.position);
            if (IsValidCell(nextCell))
            {
                cell = nextCell;
                MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position));
                return true;
            }
            Debug.Log("BASIC Ne peut pas avancer");
            return false;
        }

        //Essayer de bouger vers direction
        private bool TryMoveOnNextCell(Vector2Int direction)
        {
            Vector2Int nextPosition = new Vector2Int(cell.position.x + direction.x, cell.position.y + direction.y);
            Cell nextCell = TilingGrid.grid.GetCell(nextPosition);
            Debug.Log("BASIC SIDE cellPos + direction == " + nextPosition);

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
        private void MoveEnemy(Vector3 direction)
        {
            if (!IsServer) return;
            Debug.Log("BASIC PLACE AVANT : " + transform.position);
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(this.gameObject, direction);
            Debug.Log("BASIC PLACE APRES : " + transform.position);
        }

        private bool IsValidCell(Cell cell)
        {
            PathfindingInvalidCell(cell);
            bool isValidBlockType = (cell.type & BlockType.EnemyWalkable) > 0;
            bool hasNoObstacle = !cell.HasTopOfCellOfType(TypeTopOfCell.Obstacle);
            bool hasNoEnemy = !cell.HasTopOfCellOfType(TypeTopOfCell.Enemy);
            Debug.Log("BASIC sees NO enemy on top : " + hasNoEnemy);

            if (!isValidBlockType)
            {
                Debug.Log("BASIC MAUVAIS BLOCKTYPE : " + cell.type);
            }
            
            
            if (!hasNoEnemy)
            {
                // hasNoEnemy = true;
                Debug.Log("BASIC sees enemy on top : " + true);
                Debug.Log("BASIC next CELL POS " + cell.position);
                Debug.Log("BASIC CELL POS: " + TilingGrid.LocalToGridPosition(transform.position));
            }

            return isValidBlockType && hasNoObstacle && hasNoEnemy && !PathfindingInvalidCell(cell);
        }
    }
}