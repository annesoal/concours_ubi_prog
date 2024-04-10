using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FisrtSelectedSetterCharacterSelect : MonoBehaviour
{
    [SerializeField] private Button bonzonButton;
    [SerializeField] private Button zombotButton;
    
    void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            EventSystem.current.firstSelectedGameObject = bonzonButton.gameObject;
            bonzonButton.Select();
        }
        else
        {
            EventSystem.current.firstSelectedGameObject = zombotButton.gameObject;
            zombotButton.Select();
        }
    }
}
