using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Grid;
using Grid.Blocks;
using Grid.Interface;
using Unity.Mathematics;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using Timer = Unity.Multiplayer.Samples.Utilities.ClientAuthority.Utils.Timer;

public class Player : NetworkBehaviour, ITopOfCell
{
    public static Player LocalInstance { get; private set; }

    private const float MinPressure = 0.3f;
    private const string SPAWN_POINT_COMPONENT_ERROR =
        "Chaque spawn point de joueur doit avoir le component `BlockPlayerSpawn`";
    [SerializeField] private float cooldown = 0.1f;
    [SerializeField] private PlayerTileSelector _selector;
    [SerializeField] private GameObject _highlighter;

    private Recorder<GameObject> _highlighters;
    private Timer _timer;

    public int EnergyAvailable
    {
        set
        {
            _totalEnergy = value;
            _currentEnergy = value;
        }
    }

    private bool _hasFinishedToMove;

    private bool _canMove;

    private int _totalEnergy;
    private int _currentEnergy;

    public Player()
    {
        _timer = new(cooldown);
        _highlighters = new();
    }
    public void InputMove(Vector2 direction)
    {
        if (IsMovementInvalid()) return;
        if (direction == Vector2.zero) return;
        if (_canMove)
            HandleInput(direction);
    }

    /// <summary>
    /// Check si les conditions de deplacement sont valides.
    /// </summary>
    /// <returns> true si elles sont invalides</returns>
    private bool IsMovementInvalid()
    {
        if (!IsOwner) return true;
        if (!CooldownHasPassed()) return true;

        return false;
    }

    private void HandleInput(Vector2 direction)
    {
        Vector2Int input = TranslateToVector2Int(direction);
        var savedSelectorPosition = SaveSelectorPosition();
        MoveType hasMoved = _selector.GetTypeOfMovement(input);

        if (hasMoved == MoveType.ConsumeLast)
        {
            IncrementEnergy();
            RemovePreviousHighlighter();
            _selector.RemoveFromRecorder();
            _selector.MoveSelector();
            _timer.Start();
        }
        else if (hasMoved == MoveType.New)
        {
            if (HasEnergy())
            {
                DecrementEnergy();
                AddHighlighter(savedSelectorPosition);
                _selector.AddToRecorder(input);
                _selector.MoveSelector();
                _timer.Start();
            }
        }
        else if (hasMoved != MoveType.Invalid)
        {
            throw new Exception("Invalide input " + direction);
        }
    }

    private Vector3 SaveSelectorPosition()
    {
        return _selector.transform.position;
    }

    private void AddHighlighter(Vector3 position)
    {
        GameObject newHighlighter = Instantiate(_highlighter, position, quaternion.identity);
        _highlighters.Add(newHighlighter);
    }
    private void RemovePreviousHighlighter()
    {
        if (!_highlighters.IsEmpty())
        {
            GameObject nextHighLighter = _highlighters.RemoveFirst();
            Destroy(nextHighLighter);
        }
    }



    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
            InputManager.Player = this;
        }

        Vector3 position;
        CharacterSelectUI.CharacterId characterSelection =
            GameMultiplayerManager.Instance.GetCharacterSelectionFromClientId(OwnerClientId);

        if (characterSelection == CharacterSelectUI.CharacterId.Monkey)
        {
            MovePlayerOnSpawnPoint(TowerDefenseManager.Instance.MonkeyBlockPlayerSpawn);
            position = transform.position;
            SetReachableCells(true, position);
        }
        else
        {
            MovePlayerOnSpawnPoint(TowerDefenseManager.Instance.RobotBlockPlayerSpawn);

            position = transform.position;
            SetReachableCells(false, position);
        }

        if (IsServer)
        {
            TilingGrid.grid.PlaceObjectAtPositionOnGrid(gameObject, position);
        }
    }
    private void MovePlayerOnSpawnPoint(Transform spawnPoint)
    {
        bool hasComponent = spawnPoint.TryGetComponent(out BlockPlayerSpawn blockPlayerSpawn);

        if (hasComponent)
        {
            blockPlayerSpawn.SetPlayerOnBlock(transform);
        }
        else
        {
            Debug.LogError(SPAWN_POINT_COMPONENT_ERROR);
        }

        if (IsOwner)
        {
            CameraController.Instance.MoveCameraToPosition(transform.position);
        }
    }
    /// <summary>
    /// Demande au timer de verifier si le temps ecoule permet un nouveau deplacement
    /// </summary>
    /// <returns></returns>
    private bool CooldownHasPassed()
    {
        return _timer.HasTimePassed();
    }

    /// <summary>
    ///  Traduite la valeur d'input en Vector2Int
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private Vector2Int TranslateToVector2Int(Vector2 value)
    {
        Vector2Int translation = new Vector2Int();
        // pas tres jolie mais franchement ca marche 
        if (value.x > MinPressure)
        {
            translation.x = 1;
        }

        else if (value.x < -MinPressure)
        {
            translation.x = -1;
        }

        else if (value.y > MinPressure)
        {
            translation.y = +1;
        }
        else if (value.y < -MinPressure)
        {
            translation.y = -1;
        }

        return translation;
    }

    public void OnConfirm()
    {
        TowerDefenseManager.Instance.SetPlayerReadyToPass(true);
        _canMove = false;
        _selector.Confirm();
        if (!_selector.isSelecting)
            _selector.Initialize(transform.position);
    }

    // Methode appellee que le joeur appuie sur le bouton de selection (A sur gamepad par defaut ou spece au clavier)
    public void OnSelect()
    {
        if (_selector.isSelecting)
            return;

        _selector.Select();
        _selector.isSelecting = true;
        _selector.Initialize(transform.position);
        TowerDefenseManager.Instance.SetPlayerReadyToPass(false);
        _canMove = true;
    }

    public IEnumerator Move()
    {
        Vector2Int oldPosition = _selector.GetCurrentPosition();
        _selector.Disable();
        Vector2Int? nextPosition = _selector.GetNextPositionToGo();
        if (nextPosition == null) yield break;

        RemoveNextHighlighter();
        StartCoroutine(MoveToNextPosition((Vector2Int)nextPosition));
        
        // TODO cote server 
        yield return new WaitUntil(IsReadyToPickUp);
        PickUpItems((Vector2Int)nextPosition);
        TilingGrid.UpdateMovePositionOnGrid(this.gameObject, oldPosition, (Vector2Int)nextPosition);
    }

    private bool IsReadyToPickUp()
    {
        return _hasFinishedToMove;
    }

    private static void PickUpItems(Vector2Int position)
    {
        GameMultiplayerManager.Instance.PickUpResourcesServerRpc(position);
    }
    private void CleanHighlighters()
    {
        while (_highlighters != null && !_highlighters.IsEmpty())
        {
            RemoveNextHighlighter();
        }
    }
    private void RemoveNextHighlighter()
    {
        if (!_highlighters.IsEmpty())
        {
            GameObject nextHighLighter = _highlighters.RemoveLast();
            Destroy(nextHighLighter);
        }
    }
    private bool HasEnergy()
    {
        return _currentEnergy > 0;
    }

    private void ResetEnergy()
    {
        _currentEnergy = _totalEnergy;
    }

    private void DecrementEnergy()
    {
        _currentEnergy--;
    }

    private void IncrementEnergy()
    {
        _currentEnergy++;
    }

    public void OnCancel()
    {
        ResetEnergy();
        CleanHighlighters();
        _selector.ResetSelf();
        TowerDefenseManager.Instance.SetPlayerReadyToPass(false);
        _canMove = false;
    }

    private IEnumerator MoveToNextPosition(Vector2Int toPosition)
    {
        Vector3 cellLocalPosition = TilingGrid.GridPositionToLocal(toPosition);
        transform.LookAt(cellLocalPosition);
        _hasFinishedToMove = false;
        int i = 0;
        while (i < 10)
        {
            float f = ((float)i) / 10;
            transform.position = Vector3.Lerp(transform.position, cellLocalPosition, f);
            yield return new WaitForSeconds(0.05f);
            i++;
        }

        _hasFinishedToMove = true;
    }

    public void ResetPlayer(int energy)
    {
        EnergyAvailable = energy;
        _canMove = false;
        _selector.ResetSelf();
        _highlighters.Reset();
    }

    public void PrepareToMove()
    {
        _selector.GetNextPositionToGo();
    }

    public TypeTopOfCell GetType()
    {
        return TypeTopOfCell.Player;
    }

    public GameObject ToGameObject()
    {
        return this.gameObject;
    }
    private void SetReachableCells(bool isMonkey, Vector3 position)
    {
        if (isMonkey)
        {
            TilingGrid.FindReachableCellsMonkey(position);
        }
        else
        {
            TilingGrid.FindReachableCellsRobot(position);
        }
    }


}

public enum MoveType
{
    New,
    ConsumeLast,
    Invalid,
}
