using System;
using DefaultNamespace;
using Grid;
using UnityEngine;
using System.Collections;
using Random = System.Random;

namespace Ennemies
{
    public sealed class BasicEnemy : Enemy
    {
        public float lerpSpeed = 0.5f;
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
            _nextPosition3d = new Vector3();
            _currentPosition3d = transform.position;
            currentPosition2d = TilingGrid.LocalToGridPosition(_currentPosition3d);
            next_cell = new Cell();
            _cellRecorder = new Recorder<Cell>();
            _helper = new EnemyGridHelper(currentPosition2d, _cellRecorder);
            _helper.AddOnTopCell(this.gameObject);
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
                Debug.Log("total energy basic: " + energy);
                Debug.Log("ratio" + ratioMovement);
                Debug.Log("modulo" + energy % ratioMovement );
                if (energy % ratioMovement == 0)
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

        private bool IsEndOfGrid()
        {
            next_cell = TilingGrid.grid.GetCell(currentPosition2d + _avancer2d);
            return next_cell.IsNone();
        }
        
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
            ChangeEnnemyPosition3d(direction);
            ChangeEnnemyPosition2d();
            float t = lerpSpeed * Time.deltaTime; 
            transform.position = Vector3.Lerp(_currentPosition3d, _nextPosition3d, t);
            _helper.AddOnTopCell(transform.gameObject);
            _helper.AddOnTopCell(transform.gameObject);
            _cellRecorder.Add(cell);
        }
        

        /**
         *  Modifie la position 3d courante de l'ennemi
         */
        private void ChangeEnnemyPosition3d(Vector3 direction)
        {
           
            currentPosition2d = _helper.GetHelperPosition();
            _currentPosition3d = TilingGrid.GridPositionToLocal(currentPosition2d, TilingGrid.TopOfCell + 1);
            Debug.Log("POSITION AVANT: " + _currentPosition3d);
            _currentPosition3d += direction;
            Debug.Log("POSITION apres: " + _currentPosition3d);
        }

        /**
         * Modifie la position2D de l'ennemi
         * La position3d doit d'abord avoir ete mis a jour
         */
        private void ChangeEnnemyPosition2d()
        {
            currentPosition2d = TilingGrid.LocalToGridPosition(_currentPosition3d);
            _helper.SetHelperPosition(currentPosition2d);
        }
    }
}