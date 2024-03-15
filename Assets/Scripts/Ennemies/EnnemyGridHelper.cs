using System;
using Grid;
using UnityEngine;

namespace Ennemies
{
    public class EnnemyGridHelper : GridHelper
    {
        private Recorder<Cell> _recorder; // Servira a verifier les dernieres positions de l'ennemi
        
        public EnnemyGridHelper(Vector2Int position, Recorder<Cell> recorder) : base(position)
        {
            _recorder = recorder;
            _recorder.AddCell(currentCell);
        }

        /**
         * TODO Valider si fin grid
         * Verifie qu'un ennemi peut avancer
         */
        public override bool IsValidCell(Vector2Int position)
        {
            Cell cell = TilingGrid.grid.GetCell(position);
            //bool isValidBlockType = (cell.type & BlockType.Walkable) > 0;
            bool hasNoObstacle = ! ContainsObstacle(cell);
            bool isEndOfGrid =  cell.IsNone();
            return ! isEndOfGrid && hasNoObstacle; //isValidBlockType 
        }

        public override Vector2Int GetHelperPosition()
        {
            return currentCell.position;
        }

        public Vector2Int GetNeighborHelperPosition(Vector2Int direction)
        {
            Vector2Int test = new Vector2Int();
            test = currentCell.position + direction;
            return test;
        }

        public override void SetHelperPosition(Vector2Int position)
        {
            currentCell.position = position;
        }


        //TODO autre maniere de faire ?
        private bool ContainsObstacle(Cell cell)
        {
            if (cell.objectsOnTop != null)
            {
                foreach (GameObject obj in cell.objectsOnTop)
                {
                    if (obj.GetComponent<Obstacle>() != null)
                    {
                        return true;
                    }
                }
            }
            return false; 
        }
        
        public override void  AddOnTopCell(GameObject gameObject)
        {
            currentCell.AddGameObject(gameObject);
        }


    }
    
}