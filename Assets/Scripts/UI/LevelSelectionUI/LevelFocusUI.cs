using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelFocusUI : MonoBehaviour
{
    [SerializeField] private SingleLevelSelectUI levelDisplay;

    [SerializeField] private Button readyButton;
    [SerializeField] private Button cancelButton;

    private LevelSelectSO _selectedLevelSO;
    private AmuletSO _selectedAmulet;
    
    private void Awake()
    {
        readyButton.onClick.AddListener(OnReadyButtonClicked);
        
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
        
        BasicShowHide.Hide(gameObject);
    }

    private void Start()
    {
        SingleAmuletTemplateUI.OnAnySingleAmuletChose += SingleAmuletTemplateUI_OnAnySingleAmuletChose;
        
        LevelSelectionInputManager.Instance.OnLeftUI += InputManager_OnLeftUI;
        LevelSelectionInputManager.Instance.OnRightUI += InputManager_OnRightUI;
        LevelSelectionInputManager.Instance.OnUpUI += InputManager_OnUpUI;
        LevelSelectionInputManager.Instance.OnDownUI += InputManager_OnDownUI;
        
        LevelSelectionInputManager.Instance.OnSelectUI += InputManager_OnSelectUI;
    }

    public void Show(LevelSelectSO toShow)
    {
        _selectedLevelSO = toShow;
        _selectedAmulet = null;
        
        levelDisplay.Show(toShow);
        
        BasicShowHide.Show(gameObject);
        
        StartCoroutine(EnableNavEventsTimer());
    }

    private const float TIMER_ACTIVATE_NAV_EVENT = 0.08f;
    private IEnumerator EnableNavEventsTimer()
    {
        yield return new WaitForSeconds(TIMER_ACTIVATE_NAV_EVENT);
        EventSystem.current.sendNavigationEvents = true;
        readyButton.Select();
    }

    private void OnReadyButtonClicked()
    {
        if (! NetworkManager.Singleton.IsServer) { return; }
        
        EventSystem.current.SetSelectedGameObject(null);
        
        if (_selectedAmulet != null)
        {
            AmuletSelector.PlayerAmuletSelection = _selectedAmulet.ID;
        }
        
        Loader.LoadNetwork(_selectedLevelSO.levelScene);
    }

    [Header("Level Selection UI")]
    [SerializeField] private LevelSelectionUI levelSelectionUI;
    private void OnCancelButtonClicked()
    {
        BasicShowHide.Hide(gameObject);

        levelSelectionUI.Show();
    }
    
    private void SingleAmuletTemplateUI_OnAnySingleAmuletChose
        (object sender, SingleAmuletTemplateUI.OnAnySingleAmuletChoseEventArgs e)
    {
        if (_selectedAmulet == e.AmuletSo)
        {
            // Selected twice
            _selectedAmulet = null;
        }
        else
        {
            _selectedAmulet = e.AmuletSo;
        }
    }

    private void InputManager_OnLeftUI(object sender, LevelSelectionInputManager.FromServerEventArgs e)
    {
        ChangeSelectedElementUI(LevelSelectionInputManager.Input.Left, e);
    }
    
    private void InputManager_OnRightUI(object sender, LevelSelectionInputManager.FromServerEventArgs e)
    {
        ChangeSelectedElementUI(LevelSelectionInputManager.Input.Right, e);
    }
    
    private void InputManager_OnUpUI(object sender, LevelSelectionInputManager.FromServerEventArgs e)
    {
        ChangeSelectedElementUI(LevelSelectionInputManager.Input.Up, e);
    }
    
    private void InputManager_OnDownUI(object sender, LevelSelectionInputManager.FromServerEventArgs e)
    {
        ChangeSelectedElementUI(LevelSelectionInputManager.Input.Down, e);
    }
    
    private void InputManager_OnSelectUI(object sender, LevelSelectionInputManager.FromServerEventArgs e)
    {
        if (!gameObject.activeSelf) { return; }

        if (!e.SyncrhonizedCall)
        {
            LevelSelectionSynchronizer.Instance.CopyInputClientRpc(LevelSelectionInputManager.Input.Select);
        }

        if (e.SyncrhonizedCall)
        {
            if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out Button toClick))
            {
                toClick.onClick.Invoke();
            }
        }
    }
    
    private void ChangeSelectedElementUI(LevelSelectionInputManager.Input inputDirection,
        LevelSelectionInputManager.FromServerEventArgs eventArgs)
    {
        if (!gameObject.activeSelf) { return; }

        if (! eventArgs.SyncrhonizedCall)
        {
            LevelSelectionSynchronizer.Instance.CopyInputClientRpc(inputDirection);
        }
        
        if (eventArgs.SyncrhonizedCall)
        {
            Selectable currentSelectedObject = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();

            GameObject toSelect = null;

            if (inputDirection == LevelSelectionInputManager.Input.Left)
            {
                toSelect = currentSelectedObject.FindSelectable(Vector3.left).gameObject;
            }

            if (inputDirection == LevelSelectionInputManager.Input.Right)
            {
                toSelect = currentSelectedObject.FindSelectable(Vector3.right).gameObject;
            }
            
            if (inputDirection == LevelSelectionInputManager.Input.Up)
            {
                toSelect = currentSelectedObject.FindSelectable(Vector3.up).gameObject;
            }
            
            if (inputDirection == LevelSelectionInputManager.Input.Down)
            {
                toSelect = currentSelectedObject.FindSelectable(Vector3.down).gameObject;
            }

            if (toSelect != null)
            {
                EventSystem.current.SetSelectedGameObject(toSelect);
            }
        }
    }
    
}
