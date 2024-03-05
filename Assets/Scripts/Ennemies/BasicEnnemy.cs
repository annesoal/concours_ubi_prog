using DefaultNamespace;
using Grid;
using UnityEngine;

namespace Ennemies
{
    public class BasicEnnemy : Ennemy
    {
        private Vector2Int _nextPosition2d;
        private Vector3 _newPosition3d;
        private Cell _cell ;
        private EnnemyGridHelper _ennemyGridHelper;
      
        public BasicEnnemy() 
        {
            ennemyType = EnnemyType.Basic;
            currentPosition2d.x = 10;
            currentPosition2d.y = 15;
            speedEnnemy = 15;
        }

        public void Initialize()
        {
            _cell = new Cell();
            _nextPosition2d = new Vector2Int();
            _newPosition3d = new Vector3();
            _ennemyGridHelper = new EnnemyGridHelper(currentPosition2d);
        }
        
        private void Update()
        {
            Initialize();
            Move();
        }

        public override void Move()
        {
            while (state && speedEnnemy != 0) //si ennemi nest pas piege et a de l'energie
            {
                //Debug.Log("speedennemy: " + speedEnnemy);
                _nextPosition2d.x = currentPosition2d.x  ;
                _nextPosition2d.y = currentPosition2d.y -1;
                _ennemyGridHelper.SetHelperPosition(_nextPosition2d);
                 if (_ennemyGridHelper.IsValidCell(_nextPosition2d))
                 {
                    ChangeEnnemyPosition();
                 }
                 speedEnnemy -= 1;
            }
        }

        private void  ChangeEnnemyPosition()
        {
            _newPosition3d = TilingGrid.GridPositionToLocal(currentPosition2d, TilingGrid.TopOfCell+1);
            _newPosition3d.z -= 1;
            transform.position = new Vector3(_newPosition3d.x,_newPosition3d.y,_newPosition3d.z);
            Debug.Log("position ennemi : "+ _newPosition3d);
            currentPosition2d = TilingGrid.LocalToGridPosition(_newPosition3d);
        }

        public override void Corrupt()
        {
            base.Corrupt();
        }
    }
}