using System;
using Grid;
using Grid.Interface;
using UnityEngine;

namespace Ennemies
{
    public class EnemyGridHelper : GridHelper
    {
        private Recorder<Cell> _recorder; // Servira a verifier les dernieres positions de l'ennemi
        
        public EnemyGridHelper(Vector2Int position, Recorder<Cell> recorder) : base(position)
        {
            _recorder = recorder;
            _recorder.Add(currentCell);
        }

        /**
         * Verifie qu'un ennemi peut avancer
         * TODO eviter les tours
         */
        public override bool IsValidCell(Vector2Int position)
        {
            Cell cell = TilingGrid.grid.GetCell(position);
            bool isValidBlockType = (cell.type & BlockType.EnemyWalkable) > 0;
            bool hasNoObstacle = ! ContainsObstacle(cell);
            return isValidBlockType && hasNoObstacle; 
        }
        

        public override Vector2Int GetHelperPosition()
        {
            return currentCell.position;
        }

        public Vector2Int GetAdjacentHelperPosition(Vector2Int direction)
        {
            return currentCell.position + direction;
        }

        public override void SetHelperPosition(Vector2Int position)
        {
            currentCell.position = position;
        }


        //TODO autre maniere de faire ?
        private bool ContainsObstacle(Cell cell)
        {
            if (cell.ObjectsTopOfCell != null)
            {
                foreach (ITopOfCell obj in cell.ObjectsTopOfCell)
                {
                    if(obj.GetType() == TypeTopOfCell.Obstacle)
                    {
                        return true;
                    } 
                }
            }
            return false; 
        }
        


    }
    
}