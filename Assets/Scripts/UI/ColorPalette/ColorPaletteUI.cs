using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPaletteUI : MonoBehaviour
{
    public static ColorPaletteUI Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    [field: SerializeField] public ColorPaletteSO ColorPaletteSo { get; private set; }
}
