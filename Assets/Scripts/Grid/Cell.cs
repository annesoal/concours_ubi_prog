using System;
using System.Collections.Generic;
using Enemies;
using Grid.Blocks;
using Grid.Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Grid
{
    public struct Cell
    {
        public int type;
        public Vector2Int position;

        public List<ITopOfCell> ObjectsTopOfCell
        {
            get
            {
                if (_objectsTopOfCell == null)
                {
                    _objectsTopOfCell = new List<ITopOfCell>();
                    return _objectsTopOfCell;
                }
                else
                {
                    return _objectsTopOfCell;
                }
            }
            private set => _objectsTopOfCell = value;
        }
        private List<ITopOfCell> _objectsTopOfCell;

        // Check que le type soit le meme (exactement) que le type de la cellule
        public bool IsOf(int blockType)
        {
             return (this.type ^ blockType) == 0;
        }

        // Check que le type existe dans le type de la cellule
        // None donne toujours vrai
        public bool Has(int blockType)
        {
            if (blockType == BlockType.None)
                return true; 
            
            return (this.type & blockType) > 0;
        }
        
        public bool IsNone()
        {
            return this.type == 0;
        }

        public void AddGameObject(ITopOfCell objectTopOfCell)
        {
            if (ObjectsTopOfCell == null)
            {
                ObjectsTopOfCell = new();
            }

            ObjectsTopOfCell?.Add(objectTopOfCell);
        }

        public bool ContainsEnemy()
        { 
            Debug.Log(this.position);
            Debug.Log(_objectsTopOfCell == null  || _objectsTopOfCell.Count == 0);
            if (_objectsTopOfCell == null || _objectsTopOfCell.Count == 0)
                return false; 
            
            foreach (ITopOfCell objectTopOfCell in _objectsTopOfCell)
            {
                Debug.Log(objectTopOfCell.GetType());
                if (objectTopOfCell.GetType() == TypeTopOfCell.Enemy)
                    return true;
            }
            return false;
        }

        public Enemy GetEnemy()
        {
            if (_objectsTopOfCell == null)
                return null;
            foreach(ITopOfCell objectTopOfCell in _objectsTopOfCell)
            {
                if (objectTopOfCell.GetType() == TypeTopOfCell.Enemy)
                {
                    return objectTopOfCell.ToGameObject().GetComponent<Enemy>();   
                }
            }
            return null;
        }

        // Si obstacle sur cell, le retourne
        public Obstacle GetObstacle()
        {
            if (_objectsTopOfCell == null)
                return null;
            foreach(ITopOfCell objectTopOfCell in _objectsTopOfCell)
            {
                if (objectTopOfCell.GetType() == TypeTopOfCell.Obstacle)
                {
                    return objectTopOfCell.ToGameObject().GetComponent<Obstacle>();   
                }
            }
            return null;
        }

       // TODO public Building GetBuilding(){}
        
        public List<Enemy> GetEnemies()
        {
            List<Enemy> enemies = new List<Enemy>();
            if (_objectsTopOfCell == null)
                return null;
            foreach(ITopOfCell objectTopOfCell in _objectsTopOfCell)
            {
                if (objectTopOfCell.GetType() == TypeTopOfCell.Enemy)
                {
                    enemies.Add(objectTopOfCell.ToGameObject().GetComponent<Enemy>());   
                }
            }
            return enemies;
        }
        public static float Distance(Cell origin, Cell destination)
        {
            Vector3 originPosition = TilingGrid.GridPositionToLocal(origin.position);
            Vector3 destinationPosition = TilingGrid.GridPositionToLocal(destination.position);
            return Vector3.Distance(originPosition, destinationPosition);
        }
        public bool HasTopOfCellOfType( TypeTopOfCell type)
        {
            if (_objectsTopOfCell == null || _objectsTopOfCell.Count == 0)
                return false;

            foreach (var objectTopOfCell in _objectsTopOfCell)
            {
                if (objectTopOfCell.GetType() == type)
                    return true;
            }

            return false;
        }

        /// <returns>True if there's a building on the cell, except when the building is a trap.</returns>
        public bool HasNonWalkableBuilding()
        {
            foreach (ITopOfCell objectOnTop in ObjectsTopOfCell)
            {
                if (IsNonWalkableBuilding(objectOnTop))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsNonWalkableBuilding(ITopOfCell objectOnTop)
        {
            if (objectOnTop.GetType() == TypeTopOfCell.Building)
            {
                BuildableObject buildableObject = objectOnTop.ToGameObject().GetComponent<BuildableObject>();
                
                return ! buildableObject.IsWalkable();
            }
            else
            {
                return false;
            }
        }

        public bool HasObjectOfTypeOnTop(TypeTopOfCell type)
        {
            if (ObjectsTopOfCell == null)
                return false; 
            foreach (var objectOnTop in ObjectsTopOfCell)
            {
                if (objectOnTop.GetType() == type)
                    return true;
            }

            return false; 
        }
        public bool HasNotBuildingOnTop()
        {
            foreach (ITopOfCell objectOnTop in ObjectsTopOfCell)
            {
                if (objectOnTop.GetType() == TypeTopOfCell.Building)
                {
                    return false;
                }
            }

            return true;
        }
    
    }
    public static class BlockType
    {
        public const int None =               0b0000_0000_0000_0000;
        public const int Walkable =           0b0000_0000_0000_0001;
        public const int Buildable =          0b0000_0000_0000_0010;
        public const int Movable =            0b0000_0000_0000_0100;
        public const int EnemySpawnBlock =    0b0000_0000_0000_1000;
        public const int SpawnBlock =         0b0000_0000_0001_0000;
        public const int BasicBlock =         0b0000_0000_0100_0000; 
        public const int EnemyWalkable =     0b0000_0000_1000_0000; 
        public const int EnemyDestination =  0b0000_0001_0000_0000; 
        
        public static int Translate(Type type)
        {
            switch (type)
            {
                case Type.Walkable:
                    return BlockType.Walkable; 
                case Type.Buildable:
                    return BlockType.Buildable; 
                case Type.Movable:
                    return BlockType.Movable; 
                case Type.EnemySpawnBlock:
                    return BlockType.EnemySpawnBlock; 
                case Type.SpawnBlock:
                    return BlockType.SpawnBlock; 
                case Type.BasicBlock:
                    return BlockType.BasicBlock; 
                case Type.EnemyWalkable:
                    return BlockType.EnemyWalkable;
                case Type.EnemyDestination:
                    return BlockType.EnemyDestination;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
    
    public enum Type
    {
         Walkable,
         Buildable,
         Movable,
         SpawnBlock,
         BasicBlock,
         EnemyWalkable,
         EnemySpawnBlock,
         EnemyDestination,
    }
}
