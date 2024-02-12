using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetworkUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    private void Awake()
    {
        startHostButton.onClick.AddListener(() =>
        {
            Debug.Log("HOST");
            GameNetworkManager.Instance.StartHost();
            BasicShowHide.Hide(gameObject);
        });
        
        startClientButton.onClick.AddListener(() =>
        {
            Debug.Log("CLIENT");
            GameNetworkManager.Instance.StartClient();
            BasicShowHide.Hide(gameObject);
        });
    }
}
