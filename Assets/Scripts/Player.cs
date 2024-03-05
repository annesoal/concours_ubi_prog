using System;
using System.Collections;
using Grid.Blocks;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    private const float MinPressure = 0.3f; 
    private const string SPAWN_POINT_COMPONENT_ERROR =
        "Chaque spawn point de joueur doit avoir le component `BlockPlayerSpawn`";
    [SerializeField] private float cooldown = 0.1f;
    [SerializeField] private TileSelector _selector;
    private bool IsMovingSelector { get; set; }
    private Timer _timer;

    public Player()
    {
        _timer = new(cooldown);
    }
    public void Move(Vector2 direction)
    {
        if (!IsOwner) return;
        // On veut pas bouger si on bouge pas le selecteur
        if (!IsMovingSelector) return;
        // On veut pas aller trop vite !
        if (!CanMove()) return;
            
        Vector2Int input = Translate(direction);
        _selector.MoveSelector(input);
        _timer.Start();   
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            InputManager.Player = this;
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

    // Methode appellee que le joeur appuie sur le bouton de selection (A sur gamepad par defaut ou spece au clavier)
    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        
        if (IsMovingSelector)
        {
            _selector.Destroy();
            IsMovingSelector = false;
        }
        else
        {
            _selector.Initialize(transform.position); 
            IsMovingSelector = true; 
        }
    }
    
}
