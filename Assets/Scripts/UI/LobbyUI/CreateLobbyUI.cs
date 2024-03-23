using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
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
            EventSystem.current.firstSelectedGameObject.GetComponent<Selectable>().Select();
        });
        
        createPrivateLobbyButton.onClick.AddListener(() =>
        {
            EventSystem.current.SetSelectedGameObject(null);
            GameLobbyManager.Instance.CarryOutCreateLobbyProcedure(lobbyNameInputField.text.Trim(), true);
        });
        
        createPublicLobbyButton.onClick.AddListener(() =>
        {
            EventSystem.current.SetSelectedGameObject(null);
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
        
        SetFirstButtonSelect();
    }

    private void SetFirstButtonSelect()
    {
        lobbyNameInputField.Select();
    }
}
