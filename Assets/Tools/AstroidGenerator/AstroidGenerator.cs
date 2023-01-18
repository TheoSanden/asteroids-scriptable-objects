using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AstroidGenerator : MonoBehaviour
{
    [SerializeField]
    Vector2 size;
    [SerializeField, Range(0.0f, 20f)]
    int detail;
    [SerializeField, Range(0.0f, 0.09f)]
    float smoothness = 0.02f;
    private int bezierDetail = 20;
    [SerializeField, Range(0.0f, 1.0f)]
    float pointGravity;
    Vector3[] points;
    Vector3[] unitPoints;
    Vector3[] splinePoints;
    RasterizeSpline rast;
    AstroidTextureGenerator texGenerator;
    //AstroidTextureGenerator texGen;
    [ExecuteInEditMode]
    private void OnEnable()
    {
        rast = new RasterizeSpline();
        texGenerator = new AstroidTextureGenerator();
    }
    private void OnDrawGizmosSelected()
    {
        if (points == null || points.Length == 0) { return; }
        Handles.matrix = Gizmos.matrix = transform.localToWorldMatrix;
        Handles.color = Color.blue;
        Handles.DrawAAPolyLine(points);
    }
    [ContextMenu("Rasterize")]
    void Rasterize()
    {
        if (rast == null) { rast = new RasterizeSpline(); }
        if (texGenerator == null) { texGenerator = new AstroidTextureGenerator(); }
        rast.Rasterize(points);
        texGenerator.GenerateTexture();
        Texture2D output = rast.CurrentWorkload;
        TextureUtilities.FillWithTexture(output, texGenerator.CurrentWorkload, 32, 32, output.GetPixel(32, 32));
        TextureUtilities.WriteTextureToFile(output, "Output", "Assets/Tools/AstroidGenerator/Sprites/");
    }
    [ContextMenu("Generate Points")]
    void GeneratePoints()
    {
        Vector3[] p = GenerateUnitPoints();
        RandomizeMagnitude(ref p);
        FlattenPoints(ref p);
        unitPoints = p;
        splinePoints = GetSplinePoints(unitPoints);
        points = GenerateBezierPoints(splinePoints);
    }
    [ContextMenu("FlattenSpline")]
    void FlattenSpline()
    {
        FlattenPoints(ref unitPoints);
        splinePoints = GetSplinePoints(unitPoints);
        points = GenerateBezierPoints(splinePoints);
    }
    Vector3[] GenerateBezierPoints(Vector3[] _points)
    {
        Vector3 p0, p1, p2, p3;
        List<Vector3> bezPoints = new List<Vector3>();
        int startIndex = 0;
        for (int i = 0; i < detail; i++)
        {
            startIndex = i * 3;
            p0 = _points[startIndex];
            p1 = _points[startIndex + 1];
            p2 = _points[startIndex + 2];
            p3 = _points[startIndex + 3];
            BezierCurve bez = new BezierCurve(p0, p1, p2, p3, bezierDetail, EasingFunction.Ease.Linear);
            bezPoints.AddRange(bez.PointsInCurveEased);
        }
        return bezPoints.ToArray();
    }
    //In local Space
    private Vector3[] GenerateUnitPoints()
    {
        Vector3 center = Vector3.zero;
        int controlPointDetail = detail;
        Vector3[] _points = new Vector3[controlPointDetail];
        for (int i = 0; i < controlPointDetail; i++)
        {
            _points[i] = GetPointOnUnitCircle(i * (360.0f / controlPointDetail));
        }
        return _points;
    }
    private void RandomizeMagnitude(ref Vector3[] p_points)
    {
        for (int i = 0; i < p_points.Length; i++)
        {
            float avarage = (size.x + size.y / 2);
            float delta = size.y - size.x;
            float magnitude = avarage + (Random.Range(0.0f, delta) * RandomSign());
            p_points[i] *= magnitude;
        }
    }
    private void FlattenPoints(ref Vector3[] p_points)
    {
        int left, middle, right;

        for (int j = 0; j < pointGravity; j++)
        {
            for (int i = 0; i < p_points.Length; i++)
            {
                left = (i == 0) ? p_points.Length - 1 : i - 1;
                middle = i;
                right = (i == p_points.Length - 1) ? 0 : i + 1;
                float average = (p_points[left].magnitude + p_points[middle].magnitude + p_points[right].magnitude) / 3;
                //Move all points or just middle??
                float delta = average - p_points[middle].magnitude;
                float mag = p_points[middle].magnitude + (delta * pointGravity);
                p_points[middle] = p_points[middle].normalized * mag;

                /*p_points[left] = p_points[left].normalized * (p_points[left].magnitude + ((average - p_points[left].magnitude) * pointGravity));
                p_points[middle] = p_points[middle].normalized * (p_points[middle].magnitude + ((average - p_points[middle].magnitude) * pointGravity));
                p_points[right] = p_points[right].normalized * (p_points[right].magnitude + ((average - p_points[right].magnitude) * pointGravity));*/
            }
        }
    }
    private Vector3 GetPointOnUnitCircle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    private int RandomSign()
    {
        return Random.value < .5 ? 1 : -1;
    }
    private Vector3[] GetSplinePoints(Vector3[] unitPoints)
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 lastPoint = Vector3.zero;
        for (int i = 0; i < unitPoints.Length; i++)
        {
            Vector3 left, middle, right;
            left = (i == 0) ? unitPoints[unitPoints.Length - 1] : unitPoints[i - 1];
            middle = unitPoints[i];
            right = (i == unitPoints.Length - 1) ? unitPoints[0] : unitPoints[i + 1];
            Vector3[] tangentPoints = GetTangentPoints(left, middle, right);
            if (i == 0)
            {
                points.Add(unitPoints[i]);
                points.Add(tangentPoints[1]);
                lastPoint = tangentPoints[0];
                continue;
            }
            else if (i == unitPoints.Length - 1)
            {
                points.Add(tangentPoints[0]);
                points.Add(unitPoints[i]);
                points.Add(tangentPoints[1]);
                points.Add(lastPoint);
                points.Add(unitPoints[0]);
                continue;
            }
            points.Add(tangentPoints[0]);
            points.Add(middle);
            points.Add(tangentPoints[1]);
        }
        return points.ToArray();
    }
    private Vector3[] GetTangentPoints(Vector3 left, Vector3 middle, Vector3 right)
    {
        Handles.matrix = this.transform.localToWorldMatrix;
        Vector3[] tangents = new Vector3[2];
        Vector3 a = (left - middle).normalized;
        Vector3 b = (right - middle).normalized;
        Vector3 baseLine = (a - b) * (smoothness * size.y);
        Vector3 tanA = middle + baseLine;
        Vector3 tanB = middle - baseLine;
        /*
        Handles.color = Color.green;
        Handles.DrawLine(Vector3.zero, b);
        Handles.DrawLine(Vector3.zero, a);
        Handles.DrawLine(Vector3.zero,tanA);
        Handles.DrawLine(Vector3.zero, tanB);
        Handles.DrawLine(tanA, tanB);
        */
        tangents[0] = tanA;
        tangents[1] = tanB;
        return tangents;
    }

}
