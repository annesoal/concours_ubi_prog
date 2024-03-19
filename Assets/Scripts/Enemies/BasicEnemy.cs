using DefaultNamespace;
using Ennemies;
using Grid;
using UnityEngine;
using Random = System.Random;

namespace Enemies
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
        
        private bool IsEndOfGrid()
        {
            next_cell = TilingGrid.grid.GetCell(currentPosition2d + _avancer2d);
            return next_cell.IsNone();
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