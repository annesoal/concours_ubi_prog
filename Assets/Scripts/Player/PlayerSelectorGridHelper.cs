using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class PlayerSelectorGridHelper : GridHelper
    {
        // Permetterait de parcourir la File pour avoir les deplacements qui se jouent dans l'ordre durant la phase 
        // d'action
        private readonly Recorder<Cell> _recorder;

        public PlayerSelectorGridHelper(Vector2Int position2d) : base(position2d)
        {
            currentCell = TilingGrid.grid.GetCell(position2d);
        }

        public PlayerSelectorGridHelper(Vector2Int position, Recorder<Cell> recorder) : base(position)
        {
            _recorder = recorder;
            _recorder.Add(currentCell);
        }


        public override bool IsValidCell(Vector2Int position)
        {
            position = currentCell.position + position;
            var cell = TilingGrid.grid.GetCell(position);

            // Comme ca on selectionne pas le vide 
            return cell.type != BlockType.None;
        }

        public override Vector2Int GetHelperPosition()
        {
            return currentCell.position;
        }

        public override void SetHelperPosition(Vector2Int direction)
        {
            var next = currentCell.position + direction;
            currentCell = TilingGrid.grid.GetCell(next);
            _recorder.Add(currentCell);
        }

        public override void AddOnTopCell(GameObject gameObject)
        {
            currentCell.AddGameObject(gameObject);
            TilingGrid.grid.UpdateCell(currentCell);
        }

        public static List<GameObject> GetElementsOnTopOfCell(Vector2Int position)
        {
            try
            {
                var cell = TilingGrid.grid.GetCell(position);
                Debug.Log(cell.position);
                var objectsOnTop = cell.objectsOnTop;
                if (objectsOnTop == null)
                {
                    Debug.Log("List was null");
                    return new List<GameObject>();
                }

                return objectsOnTop;
            }
            catch (ArgumentException e)
            {
                Debug.Log("Got wrong position?");
                return new List<GameObject>();
            }
        }

        public static void RemoveElement(GameObject element, Vector2Int position)
        {
            try
            {
                var cell = TilingGrid.grid.GetCell(position);
                cell.objectsOnTop.Remove(element);
                TilingGrid.grid.UpdateCell(cell);
                Debug.Log(element + " has been removed");
            }
            catch (ArgumentException e)
            {
            }
        }
    }
}