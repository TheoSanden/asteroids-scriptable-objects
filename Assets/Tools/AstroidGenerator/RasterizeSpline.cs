using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using UnityEngine;

public class RasterizeSpline
{
    [SerializeField]
    const int resolution = 64;
    string fileName = "test";
    string path = "Assets/Resources/PaletteGenerator/";
    Texture2D currentWorkload;
    public void Rasterize(Vector3[] points)
    {
        Texture2D tex = new Texture2D(resolution, resolution);
        foreach (Vector3 point in points)
        {
            Vector2 pointFromOrigo = new Vector2(resolution / 2, resolution / 2);
            pointFromOrigo += new Vector2(Mathf.FloorToInt(point.x * (resolution / 2)), Mathf.FloorToInt(point.y * (resolution / 2)));
            tex.SetPixel((int)pointFromOrigo.x, (int)pointFromOrigo.y, Color.black);
        }
        Color replacementColor = tex.GetPixel(resolution / 2, resolution / 2);
        Fill(tex, resolution / 2, resolution / 2, replacementColor, Color.blue);
        currentWorkload = tex;
        WriteToFile();
    }
    public void WriteToFile()
    {
        File.WriteAllBytes(path + fileName + ".png", currentWorkload.EncodeToPNG());
    }
    private bool Fill(Texture2D workload, int x, int y, Color prevColor, Color newColor)
    {
        if (x > resolution - 1 || x < 0 || y > resolution - 1 || y < 0) return true;
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
    float ColorDeltaLength(Color a, Color b)
    {
        Vector3 colorVector = new Vector3(a.r - b.r, a.g - b.g, a.b - b.b);
        return Mathf.Abs(colorVector.magnitude);
    }
}
