using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;
    [SerializeField] private Button createPrivateLobbyButton;
    [SerializeField] private Button createPublicLobbyButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            BasicShowHide.Hide(gameObject);
        });
        
        createPrivateLobbyButton.onClick.AddListener(() =>
        {
            GameLobbyManager.Instance.CarryOutCreateLobbyProcedure(lobbyNameInputField.text.Trim(), true);
        });
        
        createPublicLobbyButton.onClick.AddListener(() =>
        {
            GameLobbyManager.Instance.CarryOutCreateLobbyProcedure(lobbyNameInputField.text.Trim(), false);
        });
    }

    private void Start()
    {
        BasicShowHide.Hide(gameObject);
    }

    public void Show()
    {
        BasicShowHide.Show(gameObject);
    }
}
