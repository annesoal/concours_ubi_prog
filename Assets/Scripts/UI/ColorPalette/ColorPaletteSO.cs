using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Palette", menuName = "UI/Color Palette")]
public class ColorPaletteSO : ScriptableObject
{
    public Color errorColor;
    
    public Color darkBackgroundTextColor;
    
    public Color alternativeDarkBackgroundTextColor;
    
    public Color lightBackgroundTextColor;
}
