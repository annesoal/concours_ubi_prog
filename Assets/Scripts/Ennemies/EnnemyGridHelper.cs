using System;
using Grid;
using UnityEngine;

namespace Ennemies
{
    public class EnnemyGridHelper : GridHelper
    {
        private CellRecorder _recorder; // Servira a verifier les dernieres positions de l'ennemi
        
        public EnnemyGridHelper(Vector2Int position, CellRecorder recorder) : base(position)
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
            bool isValidBlockType = (cell.type & BlockType.Walkable) > 0;
            bool hasNoObstacle = ContainsObstacle(cell);
            bool isNotEndOfGrid = true; // TODO
            return true; //isValidBlockType && hasNoObstacle;
        }

        public override Vector2Int GetHelperPosition()
        {
            return currentCell.position;
        }

        public override void SetHelperPosition(Vector2Int position)
        {
            currentCell.position = position;
        }

        //TODO 
        private bool IsNotEndOfGrid(Cell cell)
        {
            return true;
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
                        Debug.Log("Obstacle present = true");
                        return true;
                    }
                }
            }
            return false; 
        }


    }
    
}