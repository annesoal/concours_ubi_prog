using System;
using System.Collections.Generic;
using Ennemies;
using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;

namespace Enemies
{
    public class BigGuyEnemy : Enemy, ICorrupt<Obstacle>
    {
        private Random _rand = new();

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
         * Deplace un ennemi d'un bloc
         */
        public override void Move(int energy)
        {
            {
                if (!IsServer) return;
                if (!IsTimeToMove(energy)) return;

                if (LooksForDestruction())
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

        public bool LooksForDestruction()
        {
            if (path == null || path.Count == 0)
                return true;

            Cell nextCell = path[0];
            path.RemoveAt(0);
            Debug.Log("BIGGUY PATH next cell position" + nextCell.position);
            List<Cell> cellsInRadius = TilingGrid.grid.GetCellsInRadius(nextCell, 1);

            if (PathfindingInvalidCell(nextCell))
            {
                Obstacle obstacle = nextCell.GetObstacle();
                Corrupt(obstacle);
            }
            else
            {
                Debug.Log("BIGGUY Avance !");
                cell = nextCell;
                MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position));
                return true;
            }
            
            
            // Regarde si peut detruire quelque chose autour de lui
            // Si oui, % de chance de le faire. Si le fait, return true
            //       Si directement en face, detruit, return true (peut avancer au prochain tour, sauf si choisit de detruire a cote)
            //      Sinon, pile ou face.
            // Sinon, ou si choisit de pas detruire, Move
            return true;
        }


        public void Corrupt(Obstacle toCorrupt)
        {
            toCorrupt.Damage(3);
            Debug.Log("BIGGUY attaque obstacle");
        }
        
        

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
            Cell nextCell = path[0];
            path.RemoveAt(0);
            Debug.Log("BIGGUY PATH next cell position" + nextCell.position);
            if (true) // TODO, va probablement juste avancer
            {
                cell = nextCell;
                MoveEnemy(TilingGrid.GridPositionToLocal(nextCell.position));
                return true;
            }

            Debug.Log("BIGGUY Ne peut pas avancer");
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