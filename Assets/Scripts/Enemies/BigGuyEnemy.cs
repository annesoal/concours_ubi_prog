using System;
using System.Collections.Generic;
using Ennemies;
using Grid;
using Grid.Interface;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Enemies
{
    public class BigGuyEnemy : Enemy, ICorrupt<Obstacle>
    {
        private Random _rand = new();
        [SerializeField] private int _enemyDomage = 1;

        [SerializeField]
        private int _attackRate; // TODO different attackRate pour obstacle et tower? notamment pour amulettes

        public BigGuyEnemy()
        {
            ennemyType = EnnemyType.BigGuy;
            health = 1;
        }


        protected override void Initialize()
        {
            AddInGame(this.gameObject);
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

                if (StartMoveDecision())
                {
                    Debug.Log("BIGGUY a fait quelque chose");
                }
                else
                {
                    Debug.Log("ERROR derniere position BIGGUY: " + cell.position);
                    Debug.Log("ERROR path position " + path[0].position);
                    throw new Exception("moveside did not work, case not implemented yet !");
                }
            }
        }

        public bool StartMoveDecision()
        {
            if (path == null || path.Count == 0)
                return true;

            Cell nextCell = path[0];
            path.RemoveAt(0);
            Debug.Log("BIGGUY PATH next cell position" + nextCell.position);
            if (ChoseToAttackAround(nextCell))
                return true;

            if (nextCell.HasTopOfCellOfType(TypeTopOfCell.Obstacle))
            {
                Corrupt(nextCell.GetObstacle());
                hasPath = false;
            }
            else
            {
                cell = nextCell;
                MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position));
            }

            /*
            if (!TryMoveOnNextCell(nextCell))
            {
                if (!MoveSides())
                {
                    Debug.Log("ERROR derniere position BIGGUY: " + cell.position);
                    Debug.Log("ERROR path position " + path[0].position);
                    throw new Exception("moveside did not work, case not implemented yet !");
                }
            }
            */


            return true;
            // Regarde si peut detruire quelque chose autour de lui
            // Si oui, % de chance de le faire. Si le fait, return true
            //      Si directement en face, detruit? return true
            //      
            // Sinon, avance : si obstacle, bouge ?
        }

        //Checks si obstacle et tower a detruitre autour de lui, choisit de detruire ou non.
        //radius a 1
        private bool ChoseToAttackAround(Cell nextCell)
        {
            List<Cell> cellsInRadius = TilingGrid.grid.GetCellsInRadius(TilingGrid.LocalToGridPosition(transform.position), 1);
            foreach (var cell in cellsInRadius)
            {
                if (cell.HasTopOfCellOfType(TypeTopOfCell.Obstacle) && IsAttacking(cell.GetObstacle()))
                {
                    hasPath = false;
                    return true;
                }
                    
                // TODO pour tower
            }

            return false;
        }

        // Choisit d'attaquer selon aleatoirement
        private bool IsAttacking(Obstacle toCorrupt)
        {
            if (_rand.NextDouble() > 1 - _attackRate)
            {
                Debug.Log("BIGGUY CHOSE VIOLENCE");
                Debug.Log("BIGGUY ATTACK sa position: " + this.gameObject.transform.position);
                Debug.Log("BIGGUY ATTACK obstacle position: " + toCorrupt.transform.position);
                Corrupt(toCorrupt);
                return true;
            }

            return false;
        }

        public void Corrupt(Obstacle toCorrupt)
        {
            Debug.Log("BIGGUY a un obstacle dans les jambes");
            Debug.Log("BIGGUY ATTACK sa position: " + this.gameObject.transform.position);
            Debug.Log("BIGGUY ATTACK obstacle position: " + toCorrupt.transform.position);
            toCorrupt.Damage(_enemyDomage);
            Debug.Log("BIGGUY a attaque obstacle");
        }


        // Peut detruire obstacle et tower, toutes les cells sont valides a ce point
        public override bool PathfindingInvalidCell(Cell cellToCheck)
        {
            return false;
        }

        private bool IsTimeToMove(int energy)
        {
            return energy % ratioMovement == 0;
        }

        // Essaie de bouger vers l'avant
        private bool TryMoveOnNextCell(Cell nextCell)
        {
            if (path == null || path.Count == 0)
                return true;

            Debug.Log("BASIC PATH next cell position" + nextCell.position);
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
            PathfindingInvalidCell(cell);
            bool isValidBlockType = (cell.type & BlockType.EnemyWalkable) > 0;
            bool hasNoEnemy = !cell.HasTopOfCellOfType(TypeTopOfCell.Enemy);
            Debug.Log("BIGGUY sees NO enemy on top : " + hasNoEnemy);

            if (!isValidBlockType)
            {
                Debug.Log("BIGGUY MAUVAIS BLOCKTYPE : " + cell.type);
            }


            if (!hasNoEnemy)
            {
                // hasNoEnemy = true;
                Debug.Log("BIGGUY sees enemy on top : " + true);
                Debug.Log("BIGGUY next CELL POS " + cell.position);
                Debug.Log("BIGGUY CELL POS: " + TilingGrid.LocalToGridPosition(transform.position));
            }

            return isValidBlockType && hasNoEnemy && !PathfindingInvalidCell(cell);
        }
    }
}