using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Blocks;
using Grid.Interface;
using TMPro;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
namespace Grid
{
    public class TilingGrid : MonoBehaviour
    {
        [SerializeField] private GameObject _player;

        public const float TopOfCell = 0.51f;

        public static List<Cell> _monkeyReachableCells = new List<Cell>();

        public static List<Cell> _robotReachableCells = new List<Cell>();

        // A changer au besoin
        static public TilingGrid grid { get; private set; }

        public const int Size = 100;
        private readonly Cell[,] _cells = new Cell[Size, Size];

        [SerializeField] private GameObject _ground;

        public static void ResetReachableCells()
        {
            _monkeyReachableCells = new List<Cell>();
            _robotReachableCells = new List<Cell>();
        }

        public static void FindReachableCellsMonkey(Vector3 position)
        {
            _monkeyReachableCells = FindReachableCells(position);
        }

        public static void FindReachableCellsRobot(Vector3 position)
        {
            _robotReachableCells = FindReachableCells(position);
        }

        private static List<Cell> FindReachableCells(Vector3 position)
        {
            SearchAllCells searcher = new();
            Vector2Int gridPosition = LocalToGridPosition(position);
            return searcher.FindAllCells(grid.GetCell(gridPosition), InvalidSearchCell);
        }

        private static bool InvalidSearchCell(Cell cell)
        {
            return cell.IsNone();
        }


        private void Awake()
        {
            grid = this;
        }

        void Start()
        {
            SynchronizeITopOfCell.Instance.OnElementSynchronized += SynchronizeITopOfCell_OnAddingElementSynchronized;
            
            BasicBlock[] blocks = _ground.GetComponentsInChildren<BasicBlock>();

            foreach (var block in blocks)
            {
                AddBlockAsCell(block);
            }
        }

        // Ajoute un bloc dans la liste de Cells
        private void AddBlockAsCell(BasicBlock block)
        {
            Cell cell = new Cell();

            cell.type = block.GetBlockType();

            Vector2Int position = LocalToGridPosition(block.GetPosition());
            cell.position = position;
            _cells[position.x, position.y] = cell;
        }

        // Traduit une position local a la position dans la grille 
        public static Vector2Int LocalToGridPosition(Vector3 position)
        {
            Vector2Int gridPosition = new Vector2Int
            {
                x = (int)Math.Round(position.x),
                y = (int)Math.Round(position.z)
            };
            return gridPosition;
        }


        // Donne la Cellule a la position donnee.
        public Cell GetCell(Vector2Int position)
        {
            if (PositionIsOutOfBounds(position))
                throw new ArgumentException("Aucune cellule à la position donnée.");

            return _cells[position.x, position.y];
        }

        private bool PositionIsOutOfBounds(in Vector2Int position)
        {
            return position.x < 0 || position.y < 0 || position.x >= Size || position.y >= Size;
        }

        public Cell GetCell(int x, int y)
        {
            Vector2Int position = new Vector2Int()
            {
                x = x,
                y = y,
            };
            return GetCell(position);
        }

        // Traduit une position dans la grille a une position local
        public static Vector3 GridPositionToLocal(Vector2Int position, float yPos = TopOfCell)
        {
            Vector3 localPosition = new Vector3();
            localPosition.x = position.x;
            localPosition.y = yPos;
            localPosition.z = position.y;
            return localPosition;
        }

        public static Vector3 CellPositionToLocal(Cell cell, float yPos = TopOfCell)
        {
            Vector2Int position = new Vector2Int()
            {
                x = cell.position.x,
                y = cell.position.y,
            };
            return GridPositionToLocal(position, yPos);
        }

        public List<Cell> GetCellsOfType(Type type)
        {
            List<Cell> cells = new List<Cell>();
            foreach (Cell cell in _cells)
            {
                if (cell.Has(BlockType.Translate(type)))
                {
                    cells.Add(cell);
                }
            }

            return cells;
        }

        public LinkedList<Cell> GetBuildableCells()
        {
            LinkedList<Cell> buildableCells = new LinkedList<Cell>();

            foreach (Cell cell in _cells)
            {
                if (cell.Has(BlockType.Buildable))
                {
                    buildableCells.AddLast(cell);
                }
            }

            return buildableCells;
        }

        public LinkedList<Cell> GetEnemyWalkableCells()
        {
            LinkedList<Cell> enemyWalkableCells = new LinkedList<Cell>();

            foreach (Cell cell in _cells)
            {
                if (cell.Has(BlockType.EnemyWalkable))
                {
                    enemyWalkableCells.AddLast(cell);
                }
            }

            return enemyWalkableCells;
        }
        
        /// <summary>
        /// Place object implementing ITopOfCell on the cell corresponding to destination position.
        /// The placement is synchronized on the network, thus the GameObject to place needs to be spawn
        /// before using this function.
        /// </summary>
        public void PlaceObjectAtPositionOnGrid(GameObject toPlace, Vector2Int destination)
        {
            RemoveObjectFromCurrentCell(toPlace);

            AddObjectToCellAtPosition(toPlace, destination);
        }
        
        /// <summary>
        /// Place object implementing ITopOfCell on the cell corresponding to destination position.
        /// The placement is not synchronized on the network. In other words, the changes are only applied locally.
        /// </summary>
        public void PlaceObjectAtPositionOnGridOffline(GameObject toPlace, Vector2Int destination)
        {
            // RemoveObjectFromCurrentCellOffline
            Cell sender = GetCell(LocalToGridPosition(toPlace.transform.position));
            sender.ObjectsTopOfCell.Remove(toPlace.GetComponent<ITopOfCell>());
            UpdateCell(sender);

            // AddObjectToCellAtPositionOffline
            Cell recipient = GetCell(destination);
            
            recipient.ObjectsTopOfCell.Add(toPlace.GetComponent<ITopOfCell>());

            toPlace.transform.position = GridPositionToLocal(recipient.position);
                
            UpdateCell(recipient);
        }

        public void PlaceObjectAtPositionOnGrid(GameObject toPlace, Vector3 worldPositionOfSpawn)
        {
            Vector2Int destination = LocalToGridPosition(worldPositionOfSpawn);

            PlaceObjectAtPositionOnGrid(toPlace, destination);
        }
        
        public bool HasTopOfCellOfType(Cell cell, TypeTopOfCell typeTopOfCell)
        {
            Cell cellUpdated = grid.GetCell(cell.position);
            return cellUpdated.HasTopOfCellOfType(typeTopOfCell);
        }
        
        
        private void RemoveObjectFromCurrentCell(GameObject toPlace)
        {
            Vector2Int initialGridPosition = LocalToGridPosition(toPlace.transform.position);

            RemoveElement(toPlace, initialGridPosition);
        }
        
        private void UpdateCell(Cell cell)
        {
            _cells[cell.position.x, cell.position.y] = cell;
        }
       
        ///<summary>Search in X or Y direction for a certain type cell.</summary>
        /// <param name="initialCell">Not included in the search</param>
        /// <param name="searchType"></param>
        /// <param name="searchDirection"></param>
        /// <returns>The cell of type in direction found, or the initial cell if nothing was found.</returns>
        public Cell GetCellOfTypeAtDirection(Cell initialCell, Type searchType, Vector2Int searchDirection)
        {
            Vector2Int nextCellPosition = (initialCell.position + searchDirection);

            // if not, it is Y.
            bool isXDirection = searchDirection.x != 0;

            int iInit = isXDirection ? nextCellPosition.x : nextCellPosition.y;

            ActionRef<int> forPostFunction;
            if (isXDirection)
            {
                forPostFunction = searchDirection.x < 0 ? Decrement : Increment;
            }
            else
            {
                forPostFunction = searchDirection.y < 0 ? Decrement : Increment;
            }

            Predicate<int> forCompareFunction;
            if (isXDirection)
            {
                forCompareFunction = searchDirection.x < 0 ? IsGreaterThanMimimumPosArray : IsLowerThanSize;
            }
            else
            {
                forCompareFunction = searchDirection.y < 0 ? IsGreaterThanMimimumPosArray : IsLowerThanSize;
            }

            for (int i = iInit; forCompareFunction(i); forPostFunction(ref i))
            {
                Cell currentSearch = _cells[nextCellPosition.x, nextCellPosition.y];

                if (currentSearch.Has(BlockType.Translate(searchType)))
                {
                    return currentSearch;
                }

                nextCellPosition += searchDirection;
            }

            return initialCell;
        }

        private const int MINIMUM_POSITION_OF_ARRAY = 0;

        private bool IsGreaterThanMimimumPosArray(int i)
        {
            return i > MINIMUM_POSITION_OF_ARRAY;
        }

        private bool IsLowerThanSize(int i)
        {
            return i < Size;
        }

        private void Increment(ref int i)
        {
            i++;
        }

        private void Decrement(ref int i)
        {
            i--;
        }

        private void AddObjectToCellAtPosition(GameObject toPlace, Vector2Int cellPosition)
        {
            AddElementToTopOfCellList(toPlace, cellPosition);
            
            Cell cell = GetCell(cellPosition);
            
            toPlace.transform.position = GridPositionToLocal(cell.position, TopOfCell);
        }
        
        public List<Cell> GetCellsInRadius(Cell origin, int radius)
        {
            return GetCellsInRadius(origin.position, radius);
        }

        public List<Cell> GetCellsInRadius(Vector2Int origin, int radius)
        {
            List<Cell> cells = new List<Cell>();

            int minX = Math.Max(origin.x - radius, 0);
            int maxX = Math.Min(origin.x + radius, Size - 1);
            int minY = Math.Max(origin.y - radius, 0);
            int maxY = Math.Min(origin.y + radius, Size - 1);
            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    Cell cell = GetCell(i, j);
                    if (!cell.IsNone())
                        cells.Add(cell);
                }
            }

            return cells;
        }

        private void AddElementToTopOfCellList(GameObject toAdd, Vector2Int position)
        {
            Cell toSync = GetCell(position);

            SynchronizeITopOfCell.Instance.SynchronizeAddingElement(toAdd, toSync);
        }

        public static void RemoveElement(GameObject element, Vector3 position)
        {
            try
            {
                Vector2Int gridPosition = LocalToGridPosition(position);
                RemoveElement(element, gridPosition);
            }
            catch (ArgumentException)
            {
            }
        }
        
        public static void RemoveElement(GameObject element, Vector2Int position)
        {
            try
            {
                Cell toSync = grid.GetCell(position);
                SynchronizeITopOfCell.Instance.SynchronizeRemovingElement(element, toSync);
            }
            catch (ArgumentException)
            {
            }
        }

        
        private void SynchronizeITopOfCell_OnAddingElementSynchronized
            (object sender, SynchronizeITopOfCell.OnElementSynchronizedEventArgs e)
        {
            UpdateCell(e.ToUpdate);
        }

        private delegate void ActionRef<T>(ref T item);
        //--------------------------------------------------------------------------------------------------------------
        // Fonctions utilitaires pour le deboggage ! 
        void DebugCells()
        {
            foreach (var cell in _cells)
            {
                Debug.Log("is of type None" + cell.IsOf(BlockType.None));
                if (cell.type != BlockType.None)
                {
                    Debug.Log("type : " + cell.type + " a " + cell.position.x + ", " + cell.position.y);
                    Debug.Log("is of type Walkable" + cell.IsOf(BlockType.Walkable));
                    Debug.Log("is of type Walkable or Movable" + cell.IsOf(BlockType.Walkable | BlockType.Movable));
                }
            }
        }
    }
}