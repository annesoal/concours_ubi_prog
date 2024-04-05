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
    
    [Header("Colors")]
    [SerializeField] private Color originalColor;
    [SerializeField] private Color chosenColor;

    private AmuletSO _associatedAmuletSo;

    private bool _chosen = false;

    private void Awake()
    {
        amuletSelectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    private void Start()
    {
        SingleAmuletTemplateUI.OnAnySingleAmuletChose += SingleAmuletTemplateUI_OnAnySingleAmuletChose;
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
        if (_chosen)
        {
            // revert back to original
            SetIconColor(originalColor);
            _chosen = false;
        }
        else
        {
            SetIconColor(chosenColor);
            _chosen = true;
        }
        
        OnAnySingleAmuletChose?.Invoke(this, new OnAnySingleAmuletChoseEventArgs
        {
            AmuletSo = _associatedAmuletSo,
        });
    }
    
    private void SingleAmuletTemplateUI_OnAnySingleAmuletChose(object sender, OnAnySingleAmuletChoseEventArgs e)
    {
        if (e.AmuletSo != _associatedAmuletSo)
        {
            SetIconColor(originalColor);

            _chosen = false;
        }
    }

    private void SetIconColor(Color toSet)
    {
        ColorBlock colorBlock = amuletSelectButton.colors;
        colorBlock.normalColor = toSet;
        amuletSelectButton.colors = colorBlock;
    }

    public static void ResetStaticData()
    {
        OnAnySingleAmuletChose = null;
    }
}
