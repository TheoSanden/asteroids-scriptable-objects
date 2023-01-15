using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using UnityEngine;

public class RasterizeSpline
{
    [SerializeField]
    int resolution = 64;
    string fileName = "test";
    string path = "Assets/Resources/PaletteGenerator/";
    Texture2D currentWorkload;
    public void Rasterize(Vector3[] points)
    {
        Texture2D tex = new Texture2D(resolution, resolution);
        foreach (Vector3 point in points)
        {
            Debug.Log(point);
            Vector2 pointFromOrigo = new Vector2(resolution/2, resolution/2);
            pointFromOrigo += new Vector2(Mathf.FloorToInt(point.x * (resolution/2)), Mathf.FloorToInt(point.y * (resolution/2)));
            Debug.Log(pointFromOrigo);
            tex.SetPixel((int)pointFromOrigo.x, (int)pointFromOrigo.y, Color.black);
        }
        currentWorkload = tex;
    }
    public void WriteToFile()
    {
        File.WriteAllBytes(path + fileName + ".png", currentWorkload.EncodeToPNG());
    }
}
