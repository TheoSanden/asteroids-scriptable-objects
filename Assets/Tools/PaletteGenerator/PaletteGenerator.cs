using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PaletteGenerator : MonoBehaviour
{
    [SerializeField]
    Material cameraBlitMaterial;
    [SerializeField]
    Color color0, color1, color2, color3, color4, color5;
    Texture2D _StandardLut;
    [SerializeField]
    string fileName;
    string path = "Assets/Resources/PaletteGenerator/";
    Texture2D tex = null;
    private void OnValidate()
    {
        if(tex == null) { tex = new Texture2D(6, 1); }
        Debug.Log(color0);
        tex.SetPixel(0, 0, color0);
        tex.SetPixel(1, 0, color1);
        tex.SetPixel(2, 0, color2);
        tex.SetPixel(3, 0, color3);
        tex.SetPixel(4, 0, color4);
        tex.SetPixel(5, 0, color5);
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        cameraBlitMaterial.SetTexture("_ReplacementLut", tex);
    }
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
    [ContextMenu("Generate")]
    public void Generate()
    {
        if(fileName == "Lut_Standard") { Debug.LogWarning("Can't overload Standard Lut"); }
        File.WriteAllBytes(path + fileName + ".png", tex.EncodeToPNG());
        SetImportSettings(path + fileName + ".png");
    }
    private void SetImportSettings(string path)
    {
        AssetDatabase.Refresh();
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
        importer.isReadable = true;
        importer.textureType = TextureImporterType.Default;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;
        importer.npotScale = TextureImporterNPOTScale.None;
        TextureImporterSettings importerSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(importerSettings);
        importer.SetTextureSettings(importerSettings);
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
    }
}
