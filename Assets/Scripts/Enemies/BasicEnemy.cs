
using System;
using Enemies;
using Ennemies;
using Grid;
using Grid.Interface;
using UnityEngine;

using Random = System.Random;

namespace Enemies
{
    public sealed class BasicEnemy : Enemy
    {
        private Random _rand = new();
        private Cell next_cell;

        public BasicEnemy()
        {
            ennemyType = EnnemyType.Basic;
            health = 1;
        }
        

        protected override void Initialize()
        {
            currentPosition2d = TilingGrid.LocalToGridPosition(transform.position);
            cell = TilingGrid.grid.GetCell(currentPosition2d);
            next_cell = new Cell();
            _cellRecorder = new Recorder<Cell>();
            _helper = new EnemyGridHelper(currentPosition2d, _cellRecorder);
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
                if (!IsTimeToMove(energy)) return;
                if (!IsEndOfGrid()) 
                {
                    if (!TryMoveOnNextCell())
                    {
                        if (!MoveSides())
                        {
                            throw new Exception("moveside did not work, case not implemented yet !");
                        }
                    }
                } else
                {
                    Destroy(this.gameObject);
                }
            }
        }

        public override bool PathfindingInvalidCell(Cell cellToCheck)
        {
            return cellToCheck.HasTopOfCellOfType(TypeTopOfCell.Obstacle) ||
                   cellToCheck.HasTopOfCellOfType(TypeTopOfCell.Building);
        }

        private bool IsTimeToMove(int energy)
        {
            return energy % ratioMovement == 0;
        }

        private bool IsEndOfGrid()
        {
            try
            {
                next_cell = TilingGrid.grid.GetCell(currentPosition2d + _avancer2d);
                return next_cell.IsNone();
            }
            catch (ArgumentException)
            {
                return true;
            }
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
                if(!TryMoveOnNextCell(_droite2d))
                {
                    return TryMoveOnNextCell(_gauche2d);
                }
            }

            return false; 
        }
        
        // Besoin de direction 2d pour valider ce quil a sur la cell
        //Retourne true si a pu effectuer le deplacement
        private bool TryMoveOnNextCell()
        {
            Cell nextCell = path[0];
            if (_helper.IsValidCell(nextCell.position))
            {
                cell = nextCell; 
                MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position));
                return true;
            }
            return false;
        }

        private bool TryMoveOnNextCell(Vector2Int direction)
        {
            Vector2Int nextPosition = cell.position + direction;
            
             if (_helper.IsValidCell(nextPosition))
             {
                cell = TilingGrid.grid.GetCell(nextPosition);
                MoveEnemy(TilingGrid.GridPositionToLocal(nextPosition));
                return true;
            }
            return false;
        }

        /*
         * Bouge l'ennemi
         * Enregistre sa nouvelle position dans le recorder
         */
        private void MoveEnemy(Vector3 direction)
        {
            Vector3 nextPosition = transform.position + direction;
            TilingGrid.RemoveElement(this.gameObject, transform.position); 
            transform.position = nextPosition;
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(this.gameObject, transform.position);
            _cellRecorder.Add(cell);
        }
        
    }
}