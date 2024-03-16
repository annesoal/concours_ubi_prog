using DefaultNamespace;
using Grid;
using UnityEngine;
using Random = System.Random;

namespace Ennemies
{
    public class BasicEnnemy : Ennemy
    {
        public float lerpSpeed = 0.5f;
        private Random _rand = new();

        public BasicEnnemy()
        {
            ennemyType = EnnemyType.Basic;
            health = 1;
        }
        
        /**
         * Deplace un ennemi d'un block :
         *      Vers l'avant si aucun obstacle
         *      Gauche ou droite si un obstacle
         */
        public override void Move()
        {
            if (!IsEndOfGrid())
            {
                if (!MoveInDirection(_avancer2d, _avancer))
                {
                    MoveSides();
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
           
        }
        

        private void MoveSides()
        {
            // Algo temporaire
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
            Cell next_cell = new Cell();
            next_cell = TilingGrid.grid.GetCell(currentPosition2d + _avancer2d);
            return next_cell.IsNone();
        }
        
        private bool MoveInDirection(Vector2Int direction2d, Vector3 direction)
        {
            _nextPosition2d = _helper.GetAdjacentHelperPosition(direction2d);
            if (_helper.IsValidCell(_nextPosition2d))
            {
                MoveEnnemy(direction);
                return true;
            }
            return false;
        }
        

        /*
         * Bouge l'ennemi
         * Enregistre sa nouvelle position dans le recorder
         */
        private void MoveEnnemy(Vector3 direction)
        {
            ChangeEnnemyPosition3d(direction);
            ChangeEnnemyPosition2d();
            float t = Mathf.Clamp01(lerpSpeed * Time.deltaTime); // Normalisation de la vitesse
            transform.position = Vector3.Lerp(_currentPosition3d, _nextPosition3d, t);
            _helper.AddOnTopCell(transform.gameObject);
            _cellRecorder.AddCell(cell);
            remainingMove -= 1;
        }

        /**
         *  Modifie la position 3d courante de l'ennemi
         */
        private void ChangeEnnemyPosition3d(Vector3 direction)
        {
            currentPosition2d = _helper.GetHelperPosition();
            _currentPosition3d = TilingGrid.GridPositionToLocal(currentPosition2d, TilingGrid.TopOfCell + 1);
            _currentPosition3d += direction;
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