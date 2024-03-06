using DefaultNamespace;
using Grid;
using UnityEngine;
using System.Collections;

namespace Ennemies
{
    public class BasicEnnemy : Ennemy
    {
        private Vector2Int _nextPosition2d;
        private Vector3 _currentPosition3d;
        private Cell _cell;
        private EnnemyGridHelper _ennemyGridHelper;
        private Vector3 _avancer = new Vector3(0, 0, -1);
        private Vector3 _gauche = new Vector3(-1, 0, 0);
        private Vector3 _droite = new Vector3(1, 0, 0);

        public BasicEnnemy()
        {
            ennemyType = EnnemyType.Basic;
            currentPosition2d.x = 10;
            currentPosition2d.y = 15;
            speedEnnemy = 15; //Nombre de blocs avancer par tour
        }


        public void Initialize()
        {
            _cell = new Cell();
            _nextPosition2d = new Vector2Int();
            _currentPosition3d = new Vector3();
            _cellRecorder = new CellRecorder();
            _ennemyGridHelper = new EnnemyGridHelper(currentPosition2d, _cellRecorder);
        }

        private void Update()
        {
            Initialize();
            while (state && speedEnnemy != 0)
            {
                Move();
                speedEnnemy -= 1; // facon de faire temporaire
            }
        }

        public override void Move()
        {
            SetNextPositionAhead();
            if (_ennemyGridHelper.IsValidCell(_nextPosition2d)) //Sil peut avancer
            {
                MoveEnnemy(_avancer);
            }
            else
            {
                // Verifie random sil peut aller a gauche/droite ou a droite/gauche
                // exemple : verifie gauche, obstacle, verifie droite, ok, va a droite
                //         : verifie droite, ok, va a droite (non verifie gauche...)
            }
        }


        /**
         * Avance le Helper sur la Cell en avant de l'ennemi en changeant
         * la nextPosition2d
         */
        private void SetNextPositionAhead()
        {
            _nextPosition2d.x = currentPosition2d.x;
            _nextPosition2d.y = currentPosition2d.y - 1; // -1 = Avancer
            _ennemyGridHelper.SetHelperPosition(_nextPosition2d);
        }

        /*
         * Bouge l'ennemi
         * Enregistre sa nouvelle position dans le recorder
         */
        private void MoveEnnemy(Vector3 direction)
        {
            ChangeEnnemyPosition3d(direction);
            ChangeEnnemyPosition2d();
            transform.position = new Vector3(_currentPosition3d.x, _currentPosition3d.y, _currentPosition3d.z);
            _cellRecorder.AddCell(_cell);
        }

        /**
         *  Modifie la position 3d de l'ennemi
         */
        private void ChangeEnnemyPosition3d(Vector3 direction)
        {
            _currentPosition3d = TilingGrid.GridPositionToLocal(currentPosition2d, TilingGrid.TopOfCell + 1);
            //_currentPosition3d.x += 0;
            //_currentPosition3d.z += - 1;
            _currentPosition3d += direction;
        }

        /**
         * Modifie la position2D de l'ennemi
         * La position3d doit d'abord avoir ete mis a jour
         */
        private void ChangeEnnemyPosition2d()
        {
            currentPosition2d = TilingGrid.LocalToGridPosition(_currentPosition3d);
            _cell.position = currentPosition2d;
        }

        public override void Corrupt()
        {
            base.Corrupt();
        }
    }
}