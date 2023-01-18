using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using UnityEngine;

public class TextureUtilities
{
    public static void WriteTextureToFile(Texture2D workload, string name, string path)
    {
        if (!workload) return;
        File.WriteAllBytes(path + name + ".png", workload.EncodeToPNG());
    }
    private static bool Fill(Texture2D workload, int x, int y, Color prevColor, Color newColor)
    {
        if (x > workload.width - 1 || x < 0 || y > workload.height - 1 || y < 0) return true;
        Color currentPixelColor = workload.GetPixel(x, y);
        //this could be a problem
        if (ColorDeltaLength(currentPixelColor, prevColor) > 0.1f) return true;
        if (ColorDeltaLength(currentPixelColor, newColor) < 0.1f) return true;
        if (ColorDeltaLength(currentPixelColor, prevColor) < 0.1f)
        {
            workload.SetPixel(x, y, newColor);
        }
        Fill(workload, x, y - 1, prevColor, newColor);
        Fill(workload, x - 1, y, prevColor, newColor);
        Fill(workload, x, y + 1, prevColor, newColor);
        Fill(workload, x + 1, y, prevColor, newColor);
        return false;
    }
    public static bool FillWithTexture(Texture2D workload, Texture2D fillTexture, int x, int y, Color prevColor)
    {
        if (x > workload.width - 1 || x < 0 || y > workload.height - 1 || y < 0) return true;
        Color currentPixelColor = workload.GetPixel(x, y);
        Color newColor = fillTexture.GetPixel(x, y);
        //this could be a problem
        if (ColorDeltaLength(currentPixelColor, prevColor) > 0.1f) return true;
        if (ColorDeltaLength(currentPixelColor, newColor) < 0.1f) return true;
        if (ColorDeltaLength(currentPixelColor, prevColor) < 0.1f)
        {
            workload.SetPixel(x, y, newColor);
        }
        FillWithTexture(workload, fillTexture, x, y - 1, prevColor);
        FillWithTexture(workload, fillTexture, x - 1, y, prevColor);
        FillWithTexture(workload, fillTexture, x, y + 1, prevColor);
        FillWithTexture(workload, fillTexture, x + 1, y, prevColor);
        return false;
    }
    public static float ColorDeltaLength(Color a, Color b)
    {
        Vector3 colorVector = new Vector3(a.r - b.r, a.g - b.g, a.b - b.b);
        return Mathf.Abs(colorVector.magnitude);
    }
}
