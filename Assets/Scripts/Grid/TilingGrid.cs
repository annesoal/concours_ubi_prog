using System;
using Grid.Blocks;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grid
{
    public class TilingGrid : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _obstacle;
        [SerializeField] private GameObject _ennemy;
        
        public const float TopOfCell = 0.51f;
        
        // A changer au besoin
        static public TilingGrid grid { get; private set; }
        
        private const int Size = 100; 
        private Cell [,] _cells = new Cell[Size, Size];

        [SerializeField] private GameObject _ground;
       
        [FormerlySerializedAs("spawnObstacles")] [FormerlySerializedAs("_spawnObstacle")] [SerializeField]
        private ObstaclesSpawner _spawnObstacles;
        
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
            _spawnObstacles.Initialize();
            _spawnObstacles.SpawnObstacles(_obstacle);
        }
        // Ajoute un bloc dans la liste de Cells
        private void AddBlockAsCell(BasicBlock block)
        {
            Cell cell = new Cell();
            
            cell.type = block.GetBlockType();

            Vector2Int position = LocalToGridPosition(block.GetPosition());
            // TODO : Refactor? car duplication de l'information
            cell.position = position;
            _cells[position.x, position.y] = cell;
        }
        
        // Traduit une position local a la position dans la grille 
        public static Vector2Int LocalToGridPosition(Vector3 position)
        {
            Vector2Int gridPosition = new Vector2Int(); 
            gridPosition.x = (int)position.x; 
            gridPosition.y = (int)position.z; 
            return gridPosition;
        }
        
        // Donne la Cellule a la position donnee.
        public Cell GetCell(Vector2Int position)
        {
            bool outOfBound = 
                position.x < 0 || position.y < 0 || position.x >= Size || position.y >= Size;
            
            if (outOfBound)
                throw new ArgumentException();
            
            return _cells[position.x,position.y];
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

        //-----------------------------------------------------------------------------------------------------------------
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
