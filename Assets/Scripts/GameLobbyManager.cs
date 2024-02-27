using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Random = UnityEngine.Random;

public class GameLobbyManager : MonoBehaviour
{
    public static GameLobbyManager Instance {get; private set; }

    private Lobby _joinedLobby;

    private void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad(gameObject);

        InitializeUnityAuthentication();
    }

    private void Update()
    {
        HandleHearthbeat();
    }

    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();

            // Doit etre fait lors de test sur le meme ordinateur
            initializationOptions.SetProfile(Random.Range(0, 100000).ToString());

            await UnityServices.InitializeAsync(initializationOptions);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private const float HEART_BEAT_TIMER_MAX = 15f;
    private float _heartbeatTimer = HEART_BEAT_TIMER_MAX;
    
    private void HandleHearthbeat()
    {
        if (IsLobbyHost())
        {
            _heartbeatTimer -= Time.deltaTime;

            if (_heartbeatTimer <= 0f)
            {
                _heartbeatTimer = HEART_BEAT_TIMER_MAX;
                LobbyService.Instance.SendHeartbeatPingAsync(_joinedLobby.Id);
            }
        }
    }

    public event EventHandler OnCreateLobbyStarted;
    public event EventHandler OnCreateLobbyFailed;
    
    public async void CarryOutCreateLobbyProcedure(string lobbyName, bool isPrivate)
    {
        OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);

        try
        {
            await CreateLobby(lobbyName, isPrivate);

            // TODO
            // await CreateHostRelay();
            
            GameMultiplayerManager.Instance.StartHost();
            
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
        }
        
    }

    private async Task CreateLobby(string lobbyName, bool isPrivate)
    {
        CreateLobbyOptions lobbyOptions = new CreateLobbyOptions()
        {
            IsPrivate = isPrivate
        };

        _joinedLobby = await LobbyService.Instance.CreateLobbyAsync(
            lobbyName,
            GameMultiplayerManager.MAX_NUMBER_OF_PLAYERS,
            lobbyOptions
        );
    }

    public event EventHandler OnJoinStarted;
    public event EventHandler OnJoinFailed;

    public async void QuickJoin()
    {
        OnJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            _joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            
            // TODO
            // CreateClientRelay();
        
            GameMultiplayerManager.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }
    public async void JoinLobbyByCode(string lobbyCode)
    {
        OnJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            _joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
                
            // TODO
            // CreateClientRelay();
            
            GameMultiplayerManager.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public string GetLobbyName()
    {
        return _joinedLobby.Name;
    }
    
    public string GetLobbyCode()
    {
        return _joinedLobby.LobbyCode;
    }

    public async void LeaveLobby()
    {
        try
        {
            if (_joinedLobby != null)
            {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                
                _joinedLobby = null;
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void KickPlayer(string lobbyPlayerId)
    {
        try
        {
            if (IsLobbyHost())
            {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, lobbyPlayerId);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    
    private bool IsLobbyHost()
    {
        return _joinedLobby != null && _joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

}
