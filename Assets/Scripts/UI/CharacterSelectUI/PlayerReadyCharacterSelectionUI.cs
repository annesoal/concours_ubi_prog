using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerReadyCharacterSelectionUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI errorText;

    public const string SAME_CHARACTER_SELECTION_ERROR = "Can't have same character selection for both player !";
    public const string HAVE_TO_SELECT_CHARACTER_ERROR = "You have to select a character in order to start the game !";
    public const string MISSING_PLAYER_ERROR = "Lobby needs two players in order to start the game !";

    private void Awake()
    {
        readyButton.onClick.AddListener(() =>
        {
            GameMultiplayerManager.Instance.SetPlayerReadyCharacterSelect(true);
        });
    }

    private void Start()
    {
        GameMultiplayerManager.Instance.OnPlayerDataNetworkListChanged += GameMultiplayerManager_OnPlayerDataNetworkListChanged;
        GameMultiplayerManager.Instance.OnPlayerReadyCharacterSelectChecked += GameMultiplayerManager_OnPlayerReadyCharacterSelectChecked;
        CharacterSelectUI.OnAnyCharacterSelectChanged += CharacterSelectUI_OnAnyCharacterSelectChanged;
        
        TryMakeReadyButtonAvailable();
    }

    private void TryMakeReadyButtonAvailable()
    {
        GameMultiplayerManager.Instance.CheckPlayersCanSetReadyCharacterSelect();
    }

    private void ShowErrorMessage(string errorMessage)
    {
        errorText.text = errorMessage;
        BasicShowHide.Show(errorText.gameObject);
    }

    private void GameMultiplayerManager_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
    {
        TryMakeReadyButtonAvailable();
    }
    
    private void CharacterSelectUI_OnAnyCharacterSelectChanged(object sender, EventArgs e)
    {
        TryMakeReadyButtonAvailable();
    }
    
    private void GameMultiplayerManager_OnPlayerReadyCharacterSelectChecked
        (object sender, GameMultiplayerManager.OnPlayerReadyCharacterSelectCheckedEventArgs e)
    {
        SetReadyButtonEnabled(e.canSetReady, e.errorMessage);
    }

    private void SetReadyButtonEnabled(bool isEnabled, string errorMessage)
    {
        bool readyButtonWasEnabled = readyButton.enabled;
        readyButton.enabled = isEnabled;

        if (isEnabled)
        {
            BasicShowHide.Hide(errorText.gameObject);
        }
        else
        {
            ShowErrorMessage(errorMessage);
        }

        if (readyButtonWasEnabled && ! readyButton.enabled)
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }
    }

    
    private void OnDestroy()
    {
        GameMultiplayerManager.Instance.OnPlayerDataNetworkListChanged -= GameMultiplayerManager_OnPlayerDataNetworkListChanged;
        GameMultiplayerManager.Instance.OnPlayerReadyCharacterSelectChecked -= GameMultiplayerManager_OnPlayerReadyCharacterSelectChecked;
    }

}
