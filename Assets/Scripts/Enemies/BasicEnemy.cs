
using System;
using DefaultNamespace;
using Ennemies;
using Grid;
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
            cell = new Cell();
            _nextPosition2d = new Vector2Int();
            currentPosition2d = TilingGrid.LocalToGridPosition(transform.position);
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
                if (IsTimeToMove(energy))
                {
                    if (!IsEndOfGrid()) 
                    {
                        if (!MoveInDirection(_avancer2d, _avancer))
                        {
                            MoveSides();
                        }
                    } else
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
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
        private void MoveSides()
        {
            if (_rand.NextDouble() < 0.5)
            {
                if (!MoveInDirection(_gauche2d, _gauche))
                {
                    MoveInDirection(_droite2d, _droite);
                }
            }
            else
            {
                if(!MoveInDirection(_droite2d, _droite))
                {
                    MoveInDirection(_gauche2d, _gauche);
                }
            }
        }
        
        // Besoin de direction 2d pour valider ce quil a sur la cell
        //Retourne true si a pu effectuer le deplacement
        private bool MoveInDirection(Vector2Int direction2d, Vector3 direction)
        {
            _nextPosition2d = _helper.GetAdjacentHelperPosition(direction2d);
            if (_helper.IsValidCell(_nextPosition2d))
            {
                MoveEnemy(direction);
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
            transform.position = nextPosition;
            _helper.AddOnTopCell(transform.gameObject);
            _cellRecorder.Add(cell);
        }
        
    }
}