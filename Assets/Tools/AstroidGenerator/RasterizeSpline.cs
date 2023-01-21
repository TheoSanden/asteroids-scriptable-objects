using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using UnityEngine;

public class RasterizeSpline: ScriptableObject
{
    [SerializeField]
    const int resolution = 64;
    [SerializeField]
    Texture2D workload;
    public Texture2D Workload
    {
        get => workload;
    }
    public void Rasterize(Vector3[] points)
    {
        Texture2D tex = new Texture2D(resolution, resolution);
        for (int x = 0; x < resolution; x++) 
          {
             for (int y = 0; y < resolution; y++)
             {
                tex.SetPixel(x, y, Color.clear);
             }
        }
        foreach (Vector3 point in points)
        {
            Vector2 pointFromOrigo = new Vector2(resolution / 2, resolution / 2);
            pointFromOrigo += new Vector2(Mathf.FloorToInt(point.x * (resolution / 2)), Mathf.FloorToInt(point.y * (resolution / 2)));
            tex.SetPixel((int)pointFromOrigo.x, (int)pointFromOrigo.y, new Color(255, 255, 0));
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        workload = tex;
    }
}
