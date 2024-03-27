using System;
using System.Collections.Generic;
using Grid.Blocks;
using Grid.Interface;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grid
{
    public class TilingGrid : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        
        public const float TopOfCell = 0.51f;
        
        // A changer au besoin
        static public TilingGrid grid { get; private set; }
        
        public const int Size = 100; 
        private readonly Cell [,] _cells = new Cell[Size, Size];

        [SerializeField] private GameObject _ground;
       
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
            bool outOfBound = 
                position.x < 0 || position.y < 0 || position.x >= Size || position.y >= Size;

            if (outOfBound)
                throw new ArgumentException("Aucune cellule à la position donnée.");
            
            return _cells[position.x,position.y];
        }
        public Cell GetCell(int x, int y)
        {
            Vector2Int position = new Vector2Int(){
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

        public void PlaceObjectAtPositionOnGrid(GameObject toPlace, Vector2Int destination)
        {
            RemoveObjectFromCurrentCell(toPlace);

            AddObjectToCellAtPosition(toPlace, destination);
        }
        
        private void RemoveObjectFromCurrentCell(GameObject toPlace)
        {
            Vector2Int initialGridPosition = LocalToGridPosition(toPlace.transform.position);
            
            RemoveElement(toPlace, initialGridPosition);
        }
        
        private void AddObjectToCellAtPosition(GameObject toPlace, Vector2Int cellPosition)
        {
            Cell cell = GetCell(cellPosition);
            cell.AddGameObject(toPlace.GetComponent<ITopOfCell>());
            UpdateCell(cell);
            
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
            for (int i = minX; i <= maxX; i ++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    cells.Add(GetCell(i, j));
                }
            }
            return cells;
        }

        public static void RemoveElement(GameObject element, Vector2Int position)
        {
            try
            {
                var cell =grid.GetCell(position);
                cell.ObjectsTopOfCell.Remove(element.GetComponent<ITopOfCell>());
                grid.UpdateCell(cell);
            }
            catch (ArgumentException)
            {
            }
            
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

        //--------------------------------------------------------------------------------------------------------------
        // Fonctions utilitaires pour le deboggage ! 
        void DebugCells()
        {
            foreach (var cell in _cells)
            { 
                Debug.Log("is of type None" + cell.IsOf(BlockType.None));
                if (cell.type != BlockType.None)
                {
                    Debug.Log("type : " + cell.type + " a " + cell.position.x +  ", " + cell.position.y); 
                    Debug.Log("is of type Walkable" + cell.IsOf(BlockType.Walkable));
                    Debug.Log("is of type Walkable or Movable" + cell.IsOf(BlockType.Walkable | BlockType.Movable));
                }
            }
        }
    }
}
