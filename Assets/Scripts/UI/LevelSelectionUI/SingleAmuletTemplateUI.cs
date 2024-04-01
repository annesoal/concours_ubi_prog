using System.Collections;
using System.Collections.Generic;
using Amulets;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class SingleAmuletTemplateUI : MonoBehaviour
{
    [SerializeField] private Image amuletIcon;
    [SerializeField] private TextMeshProUGUI amuletNameText;

    public void Show(AmuletSO amuletSo)
    {
        // TODO SET ICON AND NAME BASED ON AMULET
        
        BasicShowHide.Show(gameObject);
    }
}
