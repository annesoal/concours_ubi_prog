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
        private Cell _cell ;
        private EnnemyGridHelper _ennemyGridHelper;
        
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
                _nextPosition2d.x = currentPosition2d.x  ;
                _nextPosition2d.y = currentPosition2d.y -1; // Avancer
                _ennemyGridHelper.SetHelperPosition(_nextPosition2d);
                 if (_ennemyGridHelper.IsValidCell(_nextPosition2d))
                 {
                     StartCoroutine(ChangeEnnemyPosition());
                 }
        }

        private IEnumerator ChangeEnnemyPosition()
        {
            _currentPosition3d = TilingGrid.GridPositionToLocal(currentPosition2d, TilingGrid.TopOfCell+1);
            _currentPosition3d.z -= 1;
            transform.position = new Vector3(_currentPosition3d.x,_currentPosition3d.y,_currentPosition3d.z);
            Debug.Log("position ennemi : "+ _currentPosition3d);
            currentPosition2d = TilingGrid.LocalToGridPosition(_currentPosition3d);
            yield return new WaitForSeconds(2f); 
        }

        public override void Corrupt()
        {
            base.Corrupt();
        }
    }
}