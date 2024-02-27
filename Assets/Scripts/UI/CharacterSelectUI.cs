using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button selectFistCharacterButton;
    [SerializeField] private Button selectSecondCharacterButton;
    
    // La marque est affichee lorsque le joueur selectionne le personnage correspondant.
    [SerializeField] private Image hostFirstCharacterMark;
    [SerializeField] private Image clientFirstCharacterMark;
    
    [SerializeField] private Image hostSecondCharacterMark;
    [SerializeField] private Image clientSecondCharacterMark;

    public static event EventHandler OnAnyCharacterSelectChanged;
    
    public enum CharacterId
    {
        First = 0,
        Second = 1,
        None = 2,
    }

    private void Awake()
    {
        selectFistCharacterButton.onClick.AddListener(() =>
        {
            GameMultiplayerManager.Instance.SelectCharacterVisual(CharacterId.First);
            OnAnyCharacterSelectChanged?.Invoke(this, EventArgs.Empty);
        });
        
        selectSecondCharacterButton.onClick.AddListener(() =>
        {
            GameMultiplayerManager.Instance.SelectCharacterVisual(CharacterId.Second);
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
        SetMarkActive(hostFirstCharacterMark, hostSecondCharacterMark, selectedId);
    }
    
    private void SetClientMarkActive(CharacterId selectedId)
    {
        SetMarkActive(clientFirstCharacterMark, clientSecondCharacterMark, selectedId);
    }

    private void SetMarkActive(Image fisrtCharacterMark, Image secondCharacterMark, CharacterId selectedId)
    {
        if (selectedId == CharacterId.First)
        {
            BasicShowHide.Show(fisrtCharacterMark.gameObject);
            BasicShowHide.Hide(secondCharacterMark.gameObject);
        }

        if (selectedId == CharacterId.Second)
        {
            BasicShowHide.Hide(fisrtCharacterMark.gameObject);
            BasicShowHide.Show(secondCharacterMark.gameObject);
        }

        if (selectedId == CharacterId.None)
        {
            BasicShowHide.Hide(fisrtCharacterMark.gameObject);
            BasicShowHide.Hide(secondCharacterMark.gameObject);
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
