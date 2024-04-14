using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Blocks;
using Grid.Interface;
using TMPro;
using Unity.Collections;
using Unity.Mathematics;
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

        public const int Size = 50;
        private readonly Cell[,] _cells = new Cell[Size, Size];

        [SerializeField] private GameObject _ground;
        [SerializeField] private GameObject _obstaclePlane;

        public static List<Cell> GetMonkeyReachableCells()
        {
            return _monkeyReachableCells;
        }
        
        public static List<Cell> GetRobotReachableCells()
        {
            return _robotReachableCells;
        }
        
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
            BasicBlock[] blocks = _ground.GetComponentsInChildren<BasicBlock>();
            foreach (var block in blocks)
            {
                AddBlockAsCell(block);
            }

            InitializeObstacles();
        }

        private void InitializeObstacles()
        {
            Obstacle[] obstacles = _obstaclePlane.GetComponentsInChildren<Obstacle>();
            foreach (var obstacle in obstacles)
            {
                obstacle.Initialize();
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

        public Cell GetCell(Vector3 position)
        {
            Vector2Int position2D = TilingGrid.LocalToGridPosition(position);
            return GetCell(position2D);
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

        public void UpdateCell(Cell cell)
        {
            _cells[cell.position.x, cell.position.y] = cell;
        }

        public void PlaceObjectAtPositionOnGrid(GameObject toPlace, Vector3 worldPositionOfSpawn)
        {
            Vector2Int destination = LocalToGridPosition(worldPositionOfSpawn);

            PlaceObjectAtPositionOnGrid(toPlace, destination);
        }

        public static void UpdateMovePositionOnGrid(GameObject toUpdate, Vector2Int origin, Vector2Int destination)
        {
            Cell originCell = grid.GetCell(origin);
            originCell.ObjectsTopOfCell.Remove(toUpdate.GetComponent<ITopOfCell>());
            grid.UpdateCell(originCell);
            
            Cell destinationCell = grid.GetCell(destination);
            destinationCell .ObjectsTopOfCell.Remove(toUpdate.GetComponent<ITopOfCell>());
            grid.UpdateCell(originCell);
        }
        

        
        public bool HasTopOfCellOfType(Cell cell, TypeTopOfCell typeTopOfCell)
        {
            Cell cellUpdated = grid.GetCell(cell.position);
            return cellUpdated.HasTopOfCellOfType(typeTopOfCell);
        }

        public void PlaceObjectAtPositionOnGrid(GameObject toPlace, Vector2Int destination)
        {
            RemoveObjectFromCurrentCell(toPlace);

            AddObjectToCellAtPosition(toPlace, destination);
        }
        
        
        public void RemoveObjectFromCurrentCell(GameObject toPlace)
        {
            Vector2Int initialGridPosition = LocalToGridPosition(toPlace.transform.position);

            RemoveElement(toPlace, initialGridPosition);
        }

       
        ///<summary>Search in X or Y direction for a certain type cell.</summary>
        /// <param name="initialCell">Not included in the search</param>
        /// <param name="searchType"></param>
        /// <param name="searchDirection"></param>
        /// <returns>The cell of type in direction found, or the initial cell if nothing was found.</returns>
        public Cell GetCellOfTypeAtDirection(Cell initialCell, Type searchType, Vector2Int searchDirection)
        {
            List<Cell> cells = GetCellsDirectionTriangle(initialCell, searchDirection);

            foreach (var cell in cells)
            {
                if (cell.Has(BlockType.Translate(searchType)))
                {
                    return cell;
                }
            }

            return initialCell;
        }
        
        private List<Cell> GetCellsDirectionTriangle(Cell origin, Vector2Int direction)
        {
            if (direction == Vector2Int.left)
            {
                return GetCellsHorizontalSearchTriangle(origin, -1);
            }

            if (direction == Vector2Int.right)
            {
                
                return GetCellsHorizontalSearchTriangle(origin, 1);
            }

            if (direction == Vector2Int.up)
            {
                
                return GetCellsVerticalSearchTriangle(origin, 1);
            }

            if (direction == Vector2Int.down)
            {
                
                return GetCellsVerticalSearchTriangle(origin, -1);
            }

            return new List<Cell>();
        }
        
        private List<Cell> GetCellsHorizontalSearchTriangle(Cell origin, int xDirection)
        {
            List<Cell> cellsFound = new();
            int x = origin.position.x + xDirection;
            int y = origin.position.y; 
            int prevSize;
            int nextSize;
            int numberOfWhile = 1;
            do
            {
                prevSize = cellsFound.Count;
                int minY = y - numberOfWhile;  
                int maxY = y + numberOfWhile;

                for (int i = minY; i < maxY; i++)
                {
                    try
                    {
                        Cell cell = GetCell(x, i);
                        if (!cell.IsNone())
                            cellsFound.Add(cell);
                    }
                    catch (ArgumentException)
                    {
                        
                    }
                }           
                nextSize = cellsFound.Count;
                x += xDirection;
                numberOfWhile++;
            } while (prevSize < nextSize);
            return cellsFound;
        }
        
        private List<Cell> GetCellsVerticalSearchTriangle(Cell origin, int yDirection)
        {
            
            List<Cell> cellsFound = new(); 
            int x = origin.position.x;
            int y = origin.position.y + yDirection; 
            int prevSize;
            int nextSize;
            int numberOfWhile = 1;
            do
            {
                prevSize = cellsFound.Count;
                int minX = x - numberOfWhile;  
                int maxX = x + numberOfWhile;

                for (int i = minX; i < maxX; i++)
                {
                    try
                    {
                        Cell cell = GetCell(i, y);
                        if (!cell.IsNone())
                            cellsFound.Add(cell);
                    }
                    catch (ArgumentException)
                    {
                        
                    }
                }
                
                nextSize = cellsFound.Count;
                y += yDirection;
                numberOfWhile++;
            } while (prevSize < nextSize);

            Debug.LogWarning(cellsFound.Count);
            return cellsFound;
        }

        private const int MINIMUM_POSITION_OF_ARRAY = 0;

        public void AddObjectToCellAtPosition(GameObject toPlace, Vector2Int cellPosition)
        {
            Cell cell = GetCell(cellPosition);
            cell.AddGameObject(toPlace.GetComponent<ITopOfCell>());
            UpdateCell(cell);

            toPlace.transform.position = GridPositionToLocal(cell.position, TopOfCell);
        }
        
        private void AddObjectToCellAtPosition(GameObject toPlace, Vector2Int cellPosition, float yPos = TopOfCell)
        {
            Cell cell = GetCell(cellPosition);
            cell.AddGameObject(toPlace.GetComponent<ITopOfCell>());
            UpdateCell(cell);
            
            toPlace.transform.position = GridPositionToLocal(cell.position, yPos);
        }

        public void AddObjectToCellAtPositionInit(GameObject toPlace, Vector2Int cellPosition)
        {
            Cell cell = GetCell(cellPosition);
            cell.AddGameObject(toPlace.GetComponent<ITopOfCell>());
            UpdateCell(cell);
        }
        
        public static void RemoveElement(GameObject element, Vector2Int position)
        {
            try
            {
                var cell = grid.GetCell(position);
                cell.ObjectsTopOfCell.Remove(element.GetComponent<ITopOfCell>());
                grid.UpdateCell(cell);
            }
            catch (ArgumentException)
            {
            }
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
                    {
                        cells.Add(cell);
                        if (cell.position == new Vector2Int(16, 10))
                        {
                            Debug.Log("16 ,10 added");
                        }
                    }
                }
            }

            return cells;
        }
        public static List<Cell> FindCellsInCross(Cell origin)
        {
            List<Cell> cells = new();
            cells.Add(grid.GetCell(origin.position));
            cells.Add(grid.GetCell(origin.position + Vector2Int.down));
            cells.Add(grid.GetCell(origin.position + Vector2Int.up));
            cells.Add(grid.GetCell(origin.position + Vector2Int.left));
            cells.Add(grid.GetCell(origin.position + Vector2Int.right));
            return cells;
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
        
        /// <summary>
        /// Synchronize all the list of top of cells of each cells.
        /// </summary>
        public void SyncAllTopOfCells()
        {
            foreach (Cell cell in _cells)
            {
                if (cell.ObjectsTopOfCell.Count != 0)
                {
                    SynchronizeTopOfCellList.Instance.SyncIndividualTopOfCell(cell);
                }
            }
        }
        
        /// <summary>
        /// Clears all the list of top of cells of each cells.
        /// Is applied only on the client side.
        /// </summary>
        public void ClearAllClientTopOfCells()
        {
            SynchronizeTopOfCellList.Instance.ClearAllClientTopOfCellsClientRpc();
        }

        /// <summary>
        /// Note : Should only be call by the SynchronizeTopOfCellList.
        /// It is the function who actually does the clearing.
        /// </summary>
        public void ClearAllTopOfCellsSync()
        {
            foreach (Cell toClear in _cells)
            {
                toClear.ObjectsTopOfCell.Clear();
            }
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