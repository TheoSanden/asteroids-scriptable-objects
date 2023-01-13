using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using UnityEngine;

[ExecuteInEditMode]
public class PaletteGenerator : MonoBehaviour
{
    [SerializeField]
    Color color0, color1, color2, color3, color4, color5;
    Texture2D _StandardLut;
    [SerializeField]
    string fileName;
    string path = "Assets/Resources/PaletteGenerator/";
    [ExecuteInEditMode]
    private void OnEnable()
    {
        _StandardLut = Resources.Load<Texture2D>("PaletteGenerator/Lut_Standard");
        Reset();
    }
    private void Reset()
    {
        if (!_StandardLut) { return; }
        Color[] pixels = _StandardLut.GetPixels();
        color0 = pixels[0];
        color1 = pixels[1];
        color2 = pixels[2];
        color3 = pixels[3];
        color4 = pixels[4];
        color5 = pixels[5];
    }
    public void Generate()
    {
        if(fileName == "Lut_Standard") { Debug.LogWarning("Can't overload Standard Lut"); }
        Texture2D tex = new Texture2D(6,1);
        tex.SetPixel(0, 1,color0);
        tex.SetPixel(1, 1, color1);
        tex.SetPixel(2, 1, color2);
        tex.SetPixel(3, 1, color3);
        tex.SetPixel(4, 1, color4);
        tex.SetPixel(5, 1, color5);
        File.WriteAllBytes(path + fileName + ".png", tex.EncodeToPNG());
    }
}
