using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using UnityEngine;

public class AstroidTextureGenerator
{
    const int resolution = 64;
    int lightAngle;
    int colorDivisions = 3;
    Texture2D astroidTexture;
    [ExecuteInEditMode]
    public AstroidTextureGenerator()
    {
        Debug.Log("HI");
        Texture2D resourceTexture = Resources.Load<Texture2D>("AstroidGenerator/Rasterize/AstroidBumpMap");
        astroidTexture = new Texture2D(resolution, resolution);
        int resourceResolution = resourceTexture.height;
        int pixelConversion = resourceResolution / resolution;
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                Color col = resourceTexture.GetPixel(x * pixelConversion, y * pixelConversion);
                col.r = col.g = col.b = (float)Mathf.Round(col.r * colorDivisions) / colorDivisions;
                astroidTexture.SetPixel(x, y, col);
            }
        }
        // File.WriteAllBytes("Assets/Tools/AstroidGenerator/BumpMap/" + "Preview" + ".png", astroidTexture.EncodeToPNG());
    }


}
