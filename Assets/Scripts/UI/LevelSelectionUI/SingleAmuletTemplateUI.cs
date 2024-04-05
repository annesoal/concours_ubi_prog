using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class SingleAmuletTemplateUI : MonoBehaviour
{
    [SerializeField] private Button amuletSelectButton;
    [SerializeField] private TextMeshProUGUI amuletNameText;

    private AmuletSO _associatedAmuletSo;

    private void Awake()
    {
        amuletSelectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    public void Show(AmuletSO amuletSo)
    {
        _associatedAmuletSo = amuletSo;
        
        amuletSelectButton.GetComponent<Image>().sprite = amuletSo.amuletIcon;
        amuletSelectButton.GetComponent<AmuletSelectionButton>().SetAssociatedAmuletSO(amuletSo);
        
        amuletNameText.text = amuletSo.amuletName;
        
        BasicShowHide.Show(amuletNameText.gameObject);
        BasicShowHide.Show(amuletSelectButton.gameObject);
        BasicShowHide.Show(gameObject);
    }

    public static event EventHandler<OnAnySingleAmuletChoseEventArgs> OnAnySingleAmuletChose;
    public class OnAnySingleAmuletChoseEventArgs : EventArgs
    {
        public AmuletSO AmuletSo;
    }
    
    private void OnSelectButtonClicked()
    {
        OnAnySingleAmuletChose?.Invoke(this, new OnAnySingleAmuletChoseEventArgs
        {
            AmuletSo = _associatedAmuletSo,
        });
    }

    public static void ResetStaticData()
    {
        OnAnySingleAmuletChose = null;
    }
}
