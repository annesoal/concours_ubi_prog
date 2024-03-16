using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableObjectVisuals : MonoBehaviour
{
    [SerializeField] private GameObject buildableObjectPreview;

    public void HidePreview()
    {
        buildableObjectPreview.SetActive(true);
    }

    public void ShowPreview()
    {
        buildableObjectPreview.SetActive(false);
    }
}
