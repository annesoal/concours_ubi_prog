using System;
using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;

namespace Enemies
{
    public class DoggoEnemy : Enemy
    {
        private Random _rand = new();

        public DoggoEnemy()
        {
            ennemyType = EnnemyType.Doggo;
        }


        protected override void Initialize()
        {
            AddInGame(this.gameObject);
           // TilingGrid.grid.PlaceObjectAtPositionOnGrid(this.gameObject, transform.position);
        }


        /**
         * Deplace un ennemi d'un block :
         *      Vers l'avant si aucun obstacle
         *      Gauche ou droite si un obstacle
         */
        public override void Move(int energy)
        {
            if (!IsServer) return;
            if (!IsTimeToMove(energy)) return;
            if (IsAtEndDestination())
            {
                Debug.Log("DOGGO IS AT DESTINATION");
                return;
            }
            if (!TryMoveOnNextCell())
            {
                hasPath = false;
                if (!MoveSides())
                {
                    transform.rotation = Quaternion.Euler(0f, 90f, 0f); 
                    Debug.Log("DOGGO NE PEUT PAS BOUGER");
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

        // Besoin de direction 2d pour valider ce quil a sur la cell
        //Retourne true si a pu effectuer le deplacement
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


        private bool TryMoveOnNextCell(Vector2Int direction)
        {
            Vector2Int nextPosition = new Vector2Int(cell.position.x + direction.x, cell.position.y + direction.y);
            Cell nextCell = TilingGrid.grid.GetCell(nextPosition);
          
            if (IsValidCell(nextCell))
            {
                cell = nextCell;
                MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position));
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
            Debug.Log("DOGGO PLACE AVANT : " + transform.position);
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(this.gameObject, direction);
            Debug.Log("DOGGO PLACE APRES : " + transform.position);
        }

        private bool IsValidCell(Cell cell)
        {
            Debug.Log("DOGGO cell pour containsEnemy position " + TilingGrid.GridPositionToLocal(cell.position));

            bool isValidBlockType = (cell.type & BlockType.EnemyWalkable) > 0;
            bool hasNoEnemy = !cell.ContainsEnemy();

            if (!hasNoEnemy)
            {
                // hasNoEnemy = true;
                Debug.Log("DOGGO sees enemy on top : " + true);
                Debug.Log("DOGGO next CELL POS " + cell.position);
                Debug.Log("DOGGO CELL POS: " + TilingGrid.LocalToGridPosition(transform.position));
            }

            return isValidBlockType && hasNoEnemy && !PathfindingInvalidCell(cell);
        }
    }
}