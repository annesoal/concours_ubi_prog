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
            TilingGrid.grid.PlaceObjectOnGridInitialize(this.gameObject, transform.position);
        }


        /**
         * Deplace un ennemi d'un block :
         *      Vers l'avant si aucun obstacle
         *      Gauche ou droite si un obstacle
         */
        public override void Move(int energy)
        {
            if (!IsServer) return;
            Debug.Log("BASIC DEBUT TOUR " + energy);
            if (!IsTimeToMove(energy)) return;
            if (IsAtEndDestination())
            {
                Debug.Log("BASIC IS AT DESTINATION");
                return;
            }

            if (!TryMoveOnNextCell())
            {
                hasPath = false;
                if (MoveSides())
                {
                    hasPath = false;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    Debug.Log("BASIC NE PEUT PAS BOUGER");
                }
            }

            Debug.Log("BASIC FIN TOUR " + energy);
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
            bool hasNoEnemy = !cell.HasTopOfCellOfType(TypeTopOfCell.Enemy);

            if (!hasNoEnemy)
            {
                // hasNoEnemy = true;
                Debug.Log("BASIC sees enemy on top : " + true);
                Debug.Log("BASIC next CELL POS " + cell.position);
                Debug.Log("BASIC CELL POS: " + TilingGrid.LocalToGridPosition(transform.position));
            }

            Debug.Log("BASIC isVAlidCell" + (isValidBlockType && hasNoEnemy && !PathfindingInvalidCell(cell)));
            return isValidBlockType && hasNoEnemy && !PathfindingInvalidCell(cell);
        }
    }
}