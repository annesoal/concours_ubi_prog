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

    public void Show(AmuletSO amuletSo)
    {
        amuletSelectButton.GetComponent<Image>().sprite = amuletSo.amuletIcon;
        amuletNameText.text = amuletSo.amuletName;
        
        BasicShowHide.Show(gameObject);
    }
}
