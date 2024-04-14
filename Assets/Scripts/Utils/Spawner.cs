using System;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = System.Random;
using Type = Grid.Type;

namespace Utils
{
    [Serializable]
    public class Spawner
    {
        private static float _overTheTiles = 0.5f;
        [Header("When")] [SerializeField] private TowerDefenseManager.State _timeSlot;

        [Header("IfRepeatable")] [SerializeField]
        private int _startingRound = -1;

        [SerializeField] private int _endingRound = -1;
        [SerializeField] private int _period;
        [SerializeField] private List<TypeTopOfCell> blockingElementsType;
        [Header("What")] [SerializeField] private GameObject _objectToSpawn;

        [SerializeField] private CellsToCheck _cellsToCheck;

        [SerializeField] private List<Type> _BlockTypeToSpawnOn;
        private GridHelper _helper;
        private bool _isServer;
        private Vector2Int _position;
        private int _positionInList;
        private Random _rand = new();
        private Func<bool> _predicatePosition;

        private int timeToRepeate;
        public GameObject ObjectToSpawn => _objectToSpawn;

        public void Initialize(bool isServer, int positionInList)
        {
            _positionInList = positionInList;
            _isServer = isServer;
            _position = new Vector2Int();
            _helper = new SpawnerGridHelper(_position, _BlockTypeToSpawnOn);
            timeToRepeate = _period;
            _predicatePosition = CreatePositionPredicate();
        }

        private bool IsInvalidCell(Cell cell)
        {
            return cell.ObjectsTopOfCell.Count > 0; 
        }
        /// <summary>
        ///     Permet de creer un predicat pour la repetition si le _startingRound est different de -1.
        /// </summary>
        /// <returns></returns>
        private Func<bool> CreatePositionPredicate()
        {
            if (_startingRound == -1) return () => true;

            if (_endingRound == -1)
                return () =>
                {
                    if (timeToRepeate == 0)
                    {
                        timeToRepeate = _period;
                        return _startingRound <= TowerDefenseManager.Instance.currentRoundNumber;
                    }

                    timeToRepeate--;
                    return false;
                };

            return () =>
            {
                if (timeToRepeate == 0)
                {
                    timeToRepeate = _period;
                    var currentRound = TowerDefenseManager.Instance.currentRoundNumber;
                    return _startingRound <= currentRound && _endingRound >= currentRound;
                }

                timeToRepeate--;
                return false;
            };
        }

        /// <summary>
        ///     Generates the position where to put the gameObjects
        /// </summary>
        /// <returns> List of positions</returns>
        private List<Vector2Int> GeneratePositions()
        {
            List<Vector2Int> listOfPosition = new();
            var i = 0;
            do
            {
                _position.x = i;
                var j = 0;
                do
                {
                    _position.y = j;
                    _helper.SetHelperPosition(_position);
                    if (_helper.IsValidCell(_position) && RandomBool() 
                                                       && !IsInvalidCell(TilingGrid.grid.GetCell(_position))) 
                        listOfPosition.Add(_position);
                    j++;
                } while (j < TilingGrid.Size);

                i++;
            } while (i < TilingGrid.Size);

            return listOfPosition;
        }

        private List<Vector2Int> GeneratePositionBonus()
        {
            List<Vector2Int> listOfPositions = new();

            Cell cell;
            Cell robotReachableCell;
            Cell monkeyReachableCell;
            int index;
            int i = 0; 
            do
            {
                index = Math.Min((int)(new Random().NextDouble() * TilingGrid._monkeyReachableCells.Count),
                    TilingGrid._monkeyReachableCells.Count);
                monkeyReachableCell = TilingGrid._monkeyReachableCells[index];
                cell = TilingGrid.grid.GetCell(monkeyReachableCell.position);
                i++;
                if (i > 300)
                    break;
            } while (IsInvalidCell(cell));

            i = 0; 
            do 
            {
                index = Math.Min((int) (new Random().NextDouble() * TilingGrid._robotReachableCells.Count), 
                            TilingGrid._robotReachableCells.Count);
                        
                robotReachableCell = TilingGrid._robotReachableCells[index];
                cell = TilingGrid.grid.GetCell(robotReachableCell.position);
                i++;
                if (i > 300)
                    break;
            } while (IsInvalidCell(cell));
            
            listOfPositions.Add(monkeyReachableCell.position);
            listOfPositions.Add(robotReachableCell.position);
            return listOfPositions;
        }

        private bool RandomBool()
        {
            return _rand.NextDouble() > 1 - Ressource.SpawnRate;
        }

        /// <summary>
        ///     Instantiate/spawn the gameobject at the different positions
        /// </summary>
        /// <param name="listOfPosition"> Positions where to put the gameobjects</param>
        /// <param name="objectToSpawn"> GameObject to Spawn at the positions</param>
        public static void InstantiateObstacles(List<Vector2Int> listOfPosition, GameObject objectToSpawn)
        {
            foreach (var position in listOfPosition) InstantiateObstacle(position, objectToSpawn);
        }

        private static void InstantiateObstacle(Vector2Int position, GameObject objectToSpawn)
        {
            var position3d = TilingGrid.GridPositionToLocal(position);
            position3d.y += _overTheTiles;
            Object.Instantiate(objectToSpawn, position3d, Quaternion.identity);
        }

        public void AddSelfToTimeSlot(object sender,
            TowerDefenseManager.OnCurrentStateChangedEventArgs changedEventArgs)
        {
            if (changedEventArgs.newValue == _timeSlot)
                if (_predicatePosition.Invoke())
                    if (_isServer)
                    {
                        List<Vector2Int> positions;
                        switch (_cellsToCheck)
                        {
                            case CellsToCheck.EveryCells:
                                positions = GeneratePositions();
                                break;
                            case CellsToCheck.PlayerIslandCells:
                                positions = GeneratePositionBonus();
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        if (SpawnersManager.Instance == null)
                            throw new Exception("SpawnersManager instance has not been set !");
                        SpawnersManager.Instance.PlaceObjects(positions.ToArray(), _positionInList, IsInvalidCell);
                    }
        }

        public enum CellsToCheck
        {
             PlayerIslandCells,
             EveryCells,
        }
    }
}