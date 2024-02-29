using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button selectMonkeyButton;
    [SerializeField] private Button selectRobotButton;
    
    // La marque est affichee lorsque le joueur selectionne le personnage correspondant.
    [SerializeField] private Image hostMonkeyMark;
    [SerializeField] private Image clientMonkeyMark;
    
    [SerializeField] private Image hostRobotMark;
    [SerializeField] private Image clientRobotMark;

    public static event EventHandler OnAnyCharacterSelectChanged;
    
    public enum CharacterId
    {
        Monkey = 0,
        Robot = 1,
        None = 2,
    }

    private void Awake()
    {
        selectMonkeyButton.onClick.AddListener(() =>
        {
            GameMultiplayerManager.Instance.SelectCharacterVisual(CharacterId.Monkey);
            OnAnyCharacterSelectChanged?.Invoke(this, EventArgs.Empty);
        });
        
        selectRobotButton.onClick.AddListener(() =>
        {
            GameMultiplayerManager.Instance.SelectCharacterVisual(CharacterId.Robot);
            OnAnyCharacterSelectChanged?.Invoke(this, EventArgs.Empty);
        });
    }

    private void Start()
    {
        GameMultiplayerManager.Instance.OnPlayerDataNetworkListChanged += GameMultiplayerManager_OnPlayerDataNetworkListChanged;
    }

    private void GameMultiplayerManager_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
    {
        UpdateCharacterMarkVisuals();
    }

    private void UpdateCharacterMarkVisuals()
    {
        (CharacterId, CharacterId) playersSelection = GameMultiplayerManager.Instance.GetCharacterVisualSelection();
        
        SetHostMarkActive(playersSelection.Item1);
        
        SetClientMarkActive(playersSelection.Item2);
    }

    private void SetHostMarkActive(CharacterId selectedId)
    {
        SetMarkActive(hostMonkeyMark, hostRobotMark, selectedId);
    }
    
    private void SetClientMarkActive(CharacterId selectedId)
    {
        SetMarkActive(clientMonkeyMark, clientRobotMark, selectedId);
    }

    private void SetMarkActive(Image monkeyMark, Image robotMark, CharacterId selectedId)
    {
        if (selectedId == CharacterId.Monkey)
        {
            BasicShowHide.Show(monkeyMark.gameObject);
            BasicShowHide.Hide(robotMark.gameObject);
        }

        if (selectedId == CharacterId.Robot)
        {
            BasicShowHide.Hide(monkeyMark.gameObject);
            BasicShowHide.Show(robotMark.gameObject);
        }

        if (selectedId == CharacterId.None)
        {
            BasicShowHide.Hide(monkeyMark.gameObject);
            BasicShowHide.Hide(robotMark.gameObject);
        }
        
    }

    private void OnDestroy()
    {
        GameMultiplayerManager.Instance.OnPlayerDataNetworkListChanged -= GameMultiplayerManager_OnPlayerDataNetworkListChanged;
    }

    public static void ResetStaticData()
    {
        OnAnyCharacterSelectChanged = null;
    }
}
