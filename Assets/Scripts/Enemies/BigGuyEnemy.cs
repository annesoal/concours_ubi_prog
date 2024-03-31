using System;
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
    public class BigGuyEnemy : Enemy
    {
        private Random _rand = new();
        [SerializeField] private int _enemyDomage = 1;

        [SerializeField] private int _attackRate;

        public BigGuyEnemy()
        {
            ennemyType = EnnemyType.BigGuy;
        }


        protected override void Initialize()
        {
            AddInGame(this.gameObject);
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(this.gameObject, transform.position);
        }


        /**
         * Bouge aleatoirement selon les cells autour de l'ennemi.
         * - Attaquer un obstacle ou une tower
         * - Avancer (et eviter ?)
         */
        public override void Move(int energy)
        {
            {
                if (!IsServer) return;
                if (!IsTimeToMove(energy)) return;
                if (!ChoseToAttack())
                {
                    if (!TryMoveOnNextCell())
                    {
                        if (!MoveSides())
                        {
                           // transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            Debug.Log("BIGGUY NE PEUT PAS BOUGER");
                        }
                    }
                }
            }
            EmitOnAnyEnemyMoved();
        }

        public bool ChoseToAttack()
        {
            if (path == null || path.Count == 0)
                return true;
            Cell nextCell = path[0];
            path.RemoveAt(0);

            Debug.Log("BIGGUY PATH next cell position" + nextCell.position);

            List<Cell> cellsInRadius =
                TilingGrid.grid.GetCellsInRadius(TilingGrid.LocalToGridPosition(transform.position), 1);
            if (ChoseAttackObstacle(cellsInRadius)) return true;

            // TODO pour tower
            return false;
        }

        private bool ChoseAttackObstacle(List<Cell> cellsInRadius)
        {
            foreach (var aCell in cellsInRadius)
            {
                if (TilingGrid.grid.HasTopOfCellOfType(aCell, TypeTopOfCell.Obstacle) &&
                    IsAttacking(aCell.GetObstacle()))
                {
                    hasPath = false;
                    return true;
                }
            }

            return false;
        }


        // Choisit d'attaquer selon aleatoirement
        private bool IsAttacking(Obstacle toCorrupt)
        {
            if (_rand.NextDouble() > 1 - _attackRate)
            {
                toCorrupt.Damage(_enemyDomage);
                return true;
            }

            return false;
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
            Debug.Log("BIGGUY PATH next cell position" + nextCell.position);
            if (IsValidCell(nextCell))
            {
                cell = nextCell;
                MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position));
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
            Debug.Log("BIGGUY PLACE AVANT : " + transform.position);
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(this.gameObject, direction);
            Debug.Log("BIGGUY PLACE APRES : " + transform.position);
        }

        private bool IsValidCell(Cell cell)
        {
            bool isValidBlockType = (cell.type & BlockType.EnemyWalkable) > 0;
            bool hasNoEnemy = !TilingGrid.grid.HasTopOfCellOfType(cell, TypeTopOfCell.Enemy);
            bool hasNoObstacle = !TilingGrid.grid.HasTopOfCellOfType(cell, TypeTopOfCell.Obstacle);
            return isValidBlockType && hasNoEnemy && hasNoObstacle ;
        }
    }
}