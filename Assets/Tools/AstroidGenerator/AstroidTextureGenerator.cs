using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using UnityEngine;

public class AstroidTextureGenerator: ScriptableObject
{
    [SerializeField]
    const int resolution = 64;
    [SerializeField]
    float lightAngle = 0;
    int colorDivisions = 3;
    [SerializeField]
    float tilingAmount = 1;
    [SerializeField]
    Vector2 offset = new Vector2(0.5f, 0.5f);
    [SerializeField]
    Texture2D workload;
    public Texture2D Workload
    {
        get => workload;
    }
    Texture2D tiledTexture;
    Texture2D resourceTexture;
    public void GenerateTexture()
    {
        if (!resourceTexture) { resourceTexture = Resources.Load<Texture2D>("AstroidGenerator/Rasterize/AstroidBumpMap"); }
        workload = new Texture2D(resolution, resolution);
        int resourceResolution = resourceTexture.height;
        int pixelConversion = resourceResolution / resolution;
        Vector2 readOffset = new Vector2(resourceResolution * offset.x, resourceResolution * offset.y);
        Vector2 origo = new Vector2(resolution / 2, resolution / 2);
        Vector2 lightVector = GetPointInUnitCircle(lightAngle);
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {

                Color col = resourceTexture.GetPixel((int)(readOffset.x + x * pixelConversion * tilingAmount) % resourceResolution,
                                                     (int)(readOffset.y + y * pixelConversion * tilingAmount) % resourceResolution);
                Vector2 pixelPositionFromOrigo = new Vector2(x, y) - origo;
                float dotProduct = Vector2.Dot(pixelPositionFromOrigo.normalized, lightVector);
                float lerpValue = (1 + dotProduct) / 2;
                float colorValue = EasingFunction.EaseOutExpo(0, col.r, lerpValue);
                int colInt = (int)Mathf.Round(colorValue * colorDivisions);
                if (colInt == 0) colInt = 1;
                Color newColor = new Color();
                switch (colInt)
                {
                    case 1: newColor = Color.blue; break;
                    case 2: newColor = Color.red; break;
                    case 3: newColor = Color.green; break;
                }
                col = newColor;
                workload.SetPixel(x, y, col);
            }
        }
        workload.filterMode = FilterMode.Point;
        workload.Apply();
    }
    Vector2 GetPointInUnitCircle(float angle)
    {
        return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }

}
