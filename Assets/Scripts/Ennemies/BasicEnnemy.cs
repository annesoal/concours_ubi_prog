using System;
using DefaultNamespace;
using Grid;
using UnityEngine;
using System.Collections;
using Random = System.Random;

namespace Ennemies
{
    public class BasicEnnemy : Ennemy
    {
        private Vector2Int _nextPosition2d;
        private Vector3 _currentPosition3d;
        private Vector3 _nextPosition3d;

        private EnnemyGridHelper _ennemyGridHelper;
        public float lerpSpeed = 0.5f;

        // Deplacements (mettre dans parent?)
        private Vector3 _avancer = new Vector3(0, 0, -1);
        private Vector3 _gauche = new Vector3(-1, 0, 0);
        private Vector3 _droite = new Vector3(1, 0, 0);
        private Vector2Int _avancer2d = new Vector2Int(0, -1);
        private Vector2Int _gauche2d = new Vector2Int(-1, 0);
        private Vector2Int _droite2d = new Vector2Int(1, 0);
        private Random _rand = new();

        public BasicEnnemy()
        {
            ennemyType = EnnemyType.Basic;
            currentPosition2d.x = 10; // TODO
            currentPosition2d.y = 15;
            speedEnnemy = 20; //Nombre de blocs avancer par tour
        }

        private void Awake()
        {
        }

        public void Initialize()
        {
            cell = new Cell();
            _nextPosition2d = new Vector2Int();
            _currentPosition3d = new Vector3();
            _nextPosition3d = new Vector3();
            _cellRecorder = new CellRecorder();
            _ennemyGridHelper = new EnnemyGridHelper(currentPosition2d, _cellRecorder);
        }


        //TODO Enlever Update() lors du push sur Develop
        private void Update()
        {
            Initialize();
            // SetObjectOnTop(this.gameObject, currentPosition2d);
            Move();
        }

        public override void Move()
        {
            if (!MoveInDirection(_avancer2d, _avancer))
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
        }
        
        private bool MoveInDirection(Vector2Int direction2d, Vector3 direction)
        {
            _nextPosition2d = _ennemyGridHelper.GetNeighborHelperPosition(direction2d);
            if (_ennemyGridHelper.IsValidCell(_nextPosition2d))
            {
                MoveEnnemy(direction);
                return true;
            }
            return false;
        }

        /**
         * Avance le Helper sur la Cell en avant de l'ennemi en changeant
         * la nextPosition2d
         */
        private void SetNextPositionAhead() //pourrait etre refactor pour prendre direction
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
            float t = Mathf.Clamp01(lerpSpeed * Time.deltaTime); // Normalisation de la vitesse
            transform.position = Vector3.Lerp(_currentPosition3d, _nextPosition3d, t);
            _cellRecorder.AddCell(cell);
            speedEnnemy -= 1;
        }

        /**
         *  Modifie la position 3d courante de l'ennemi
         */
        private void ChangeEnnemyPosition3d(Vector3 direction)
        {
            currentPosition2d = _ennemyGridHelper.GetHelperPosition();
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
            _ennemyGridHelper.SetHelperPosition(currentPosition2d);
        }

        private void SetObjectOnTop(GameObject ennemi, Vector2Int position2d)
        {
            cell = TilingGrid.grid.GetCell(position2d);
            cell.AddGameObject(ennemi);
            // Debug.Log("REP   :" + _cell.objectsOnTop.Exists(o => o == obstacle));
        }

        public override void Corrupt()
        {
            base.Corrupt();
        }
    }
}