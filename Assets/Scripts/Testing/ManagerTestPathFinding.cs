using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Grid.Interface;
using Unity.Mathematics;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;
using Type = Grid.Type;

namespace Testing
{
    public class ManagerTestPathFinding : MonoBehaviour
    {
        private List<Cell> listOfCells = new();

        [SerializeField] private GameObject element;

        [SerializeField] private GameObject highlighter;
        // Start is called before the first frame update

        private bool hasNotStartedUpdate = true;
        private void Update()
        {
            if (hasNotStartedUpdate)
            {
                StartCoroutine(RunPathFinding());
                hasNotStartedUpdate = false;
            }

        }

        private IEnumerator RunPathFinding()
        {
            listOfCells = TilingGrid.grid.GetCellsOfType(Type.EnemyWalkable);
            while (true)
            {
                Cell cellToGoTo = GetCellToGoTo();
                Highlight(cellToGoTo);
                Debug.Log("highlighted cell was : " +  cellToGoTo.position);
                Cell origin = TilingGrid.grid.GetCell(TilingGrid.LocalToGridPosition(element.transform.position));
                List<Cell> path = AStarPathfinding.GetPath(origin, cellToGoTo, IsInvalidCellNoDebug);

                while (path.Count > 0)
                {
                    Cell toGoNext = path[0];
                    element.transform.position = TilingGrid.CellPositionToLocal(toGoNext);
                    path.RemoveAt(0);
                    yield return new WaitForSeconds(0.2f);
                }
                yield return null;
            }
        }

        private Cell GetCellToGoTo()
        {
            Cell cell;
            do
            {
                int position = (int)Random.Range(0, Math.Max(0, listOfCells.Count -1));
                cell = listOfCells[position];
                //Debug.Log( "position : " + position);
                //Debug.Log( "list size : " +  listOfCells.Count);
            } while (IsInvalidCell(cell));

            return cell;
        }

        private bool IsInvalidCell(Cell cell)
        {
            Debug.Log("cell pos " + cell.position);
            if (cell.ObjectsTopOfCell == null || cell.ObjectsTopOfCell.Count == 0)
            {
                Debug.Log("no objects on top of cell");
                return false;
            }

            if (cell.ObjectsTopOfCell.Any(objOnTop => objOnTop.GetType() == TypeTopOfCell.Obstacle))
            {
                Debug.Log("There was obj on top !");
                return true;
            };
            return false;
        }

        private bool IsInvalidCellNoDebug(Cell cell)
        {
            if (cell.ObjectsTopOfCell == null || cell.ObjectsTopOfCell.Count == 0)
            {
                return false;
            }

            return cell.ObjectsTopOfCell.Any(objOnTop => objOnTop.GetType() == TypeTopOfCell.Obstacle);
        }
        private void Highlight(Cell cell)
        {
            Instantiate(highlighter, TilingGrid.CellPositionToLocal(cell), quaternion.identity);
        }
    }
}
