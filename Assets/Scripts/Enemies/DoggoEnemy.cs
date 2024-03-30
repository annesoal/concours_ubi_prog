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
                    if (!MoveSides())
                    {
                        Debug.Log("ERROR derniere position BASIC: " + cell.position);
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
            Debug.Log("DOGGO PATH next cell position" + nextCell.position);
            if (IsValidCell(nextCell))
            {
                cell = nextCell;
                MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position));
                Debug.Log("VALID RETURN TRUE");
                return true;
            }

            Debug.Log("VALID RETURN FAlSE");
            return false;
        }

        private bool TryMoveOnNextCell(Vector2Int direction)
        {
            Vector2Int nextPosition = new Vector2Int(cell.position.x + direction.x, cell.position.y + direction.y);
            Cell nextCell = TilingGrid.grid.GetCell(nextPosition);
            Debug.Log("DOGGO SIDE cellPos + direction == " + nextPosition);

            if (IsValidCell(nextCell))
            {
                cell = nextCell;
                Debug.Log("DOGGO valid bouge side" + cell.position);
                Debug.Log("test autre position = " + nextPosition);
                
                MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position));
                return true;
            }

            hasPath = false;
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
            bool isValidBlockType = (cell.type & BlockType.EnemyWalkable) > 0;
            bool hasNoEnemy = !cell.HasTopOfCellOfType(TypeTopOfCell.Enemy);
            Debug.Log("DOGGO sees NO enemy " + hasNoEnemy);

            if (!isValidBlockType)
            {
                Debug.Log("DOGGO MAUVAIS BLOCKTYPE : " + cell.type);
            }


            if (!hasNoEnemy)
            {
                // hasNoEnemy = true;
                Debug.Log("DOGGO Has enemy on top : " + true);
                Debug.Log("DOGGO next CELL POS " + cell.position);
                Debug.Log("DOGGO CELL POS: " + TilingGrid.LocalToGridPosition(transform.position));
            }

            return isValidBlockType && hasNoEnemy && !PathfindingInvalidCell(cell);
        }
    }
}