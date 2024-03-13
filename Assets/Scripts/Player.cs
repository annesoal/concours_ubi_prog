using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Blocks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class Player : NetworkBehaviour
{
    public static Player LocalInstance { get; private set; }

    private const float MinPressure = 0.3f; 
    private const string SPAWN_POINT_COMPONENT_ERROR =
        "Chaque spawn point de joueur doit avoir le component `BlockPlayerSpawn`";
    [SerializeField] private float cooldown = 0.1f;
    [SerializeField] private TileSelector _selector;
    private bool IsMovingSelector { get; set; }
    private Timer _timer;
    public static Player Instance;

    public int Energy
    {
        set
        {
            _setEnergy = value;
            _localEnergy = value;
        }
    }

    private int _setEnergy; 
    private int _localEnergy;

    public Player()
    {
        _timer = new(cooldown);
    }
    public void MoveSelector(Vector2 direction)
    {
        if (!IsOwner) return;
        // On veut pas bouger si on bouge pas le selecteur
        if (!IsMovingSelector) return;
        // On veut pas aller trop vite !
        if (!CanMove()) return;
        if (!HasEnergy()) return; 
        HandleInput(direction);
    }
    private void HandleInput(Vector2 direction)
    {
        Vector2Int input = Translate(direction);
        _selector.MoveSelector(input);
        _timer.Start();
        if (HasMoved(direction))
        {
            _localEnergy--;
        }
    }

    private bool HasMoved(Vector2 direction)
    {
        return direction != Vector2.zero;
    }
    private bool HasEnergy()
    {
        return _localEnergy >= 0;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
            Instance = this;
        }
        
        CharacterSelectUI.CharacterId characterSelection =
            GameMultiplayerManager.Instance.GetCharacterSelectionFromClientId(OwnerClientId);

        if (characterSelection == CharacterSelectUI.CharacterId.Monkey)
        {
            MovePlayerOnSpawnPoint(TowerDefenseManager.Instance.MonkeyBlockPlayerSpawn);
        }
        else
        {
            MovePlayerOnSpawnPoint(TowerDefenseManager.Instance.RobotBlockPlayerSpawn);
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
    }
    // Demande au timer de verifier si le temps ecoule permet un nouveau deplacement
    private bool CanMove()
    {
        return _timer.HasTimePassed();
    }
    
    // Traduite la valeur d'input en Vector2Int
    private Vector2Int Translate(Vector2 value)
    {
        Vector2Int translation = new Vector2Int();
        // pas tres jolie mais franchement ca marche 
        if (value.x > MinPressure)
        {
            translation.x = 1;
        }

        if (value.x < -MinPressure)
        {
            translation.x = -1;
        }

        if (value.y > MinPressure)
        {
            translation.y = +1;
        }

        if (value.y < -MinPressure)
        {
            translation.y = -1;
        }

        return translation;
    }

    // Methode appellee quand le joeur appuie sur le bouton de selection (A sur gamepad par defaut ou spece au clavier)
    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        
        if (IsMovingSelector)
        {
            IsMovingSelector = false;
            TowerDefenseManager.Instance.SetPlayerReadyToPassTurn(true);
        }
        else
        {
            _selector.Initialize(transform.position); 
            TowerDefenseManager.Instance.SetPlayerReadyToPassTurn(false);
            IsMovingSelector = true; 
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        _localEnergy = _setEnergy;
        _selector.Reset();
       TowerDefenseManager.Instance.SetPlayerReadyToPassTurn(false); 
    }

    public void MoveCharacter()
    {
        _selector.Hide();
        _selector.MoveCharacter();
    }
}
