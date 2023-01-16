using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class BezierUtilities
{
    public delegate Vector3 GetPoint(float t);
    public static float[] GenerateDistanceLUT(GetPoint getPointDelegate, int detail)
    {
        float[] lut = new float[detail];
        lut[0] = 0;
        Vector3 lastPoint = getPointDelegate(0);
        Vector3 currentPoint = new Vector3();
        for (int i = 1; i < detail; i++)
        {
            currentPoint = getPointDelegate((float)i / detail);
            lut[i] = lut[i - 1] + Mathf.Abs((lastPoint - currentPoint).magnitude);
            lastPoint = currentPoint;
        }
        return lut;
    }

    public static float DistToT(float[] LUT, float distance)
    {
        float archLength = LUT[LUT.Length - 1];
        int n = LUT.Length;

        if (distance > 0 && distance < archLength)
        {
            for (int i = 0; i < n - 1; i++)
            {
                if (distance > LUT[i] && distance < LUT[i + 1])
                {
                    return remap(distance,
                        LUT[i],
                        LUT[i + 1],
                        (float)i / (n - 1f),
                        (i + 1) / (n - 1f)
                        );
                }
            }
        }
        return distance / archLength;
    }

    public static float remap(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }
}
[System.Serializable]
public class BezierSettings
{
    #region editor settings
    [SerializeField]
    public bool lockPositions;
    #endregion
}
public class BezierCurve
{
    public BezierCurve()
    {
        Recalculate();
    }
    public BezierCurve(Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3, int _detail, EasingFunction.Ease easing)
    {
        p0 = _p0;
        p1 = _p1;
        p2 = _p2;
        p3 = _p3;
        detail = _detail;
        PointSpacing = easing;
        Recalculate();
    }

    #region Handles
    int detail;
    [SerializeField]
    bool draw = false;
    [SerializeField, HideInInspector]
    float maxMag = 0;
    [SerializeField, HideInInspector]
    public BezierSettings settings = new BezierSettings();
    #endregion
    #region Easing
    [SerializeField, HideInInspector]
    private EasingFunction.Ease pointSpacing = EasingFunction.Ease.Linear;
    public EasingFunction.Ease PointSpacing
    {
        get => pointSpacing;
        set
        {
            pointSpacing = value; pointSpacingFunction = EasingFunction.GetEasingFunction(value); Recalculate();
        }
    }
    [SerializeField, HideInInspector]
    private EasingFunction.Function pointSpacingFunction;
    public EasingFunction.Function PointSpacingFunction
    {
        get
        {
            if (pointSpacingFunction == null) { pointSpacingFunction = EasingFunction.GetEasingFunction(pointSpacing); }
            return pointSpacingFunction;
        }
    }
    [SerializeField, HideInInspector]
    private float[] distanceLUT;
    #endregion
    #region Curves
    [SerializeField, HideInInspector]
    private Vector3[] pointsInCurve = new Vector3[0];
    [SerializeField, HideInInspector]
    private Vector3[] pointsInCurveEased = new Vector3[0];
    public Vector3[] PointsInCurveEased
    {
        get => pointsInCurveEased;
    }
    public Vector3[] PointsInCurve
    {
        get => PointsInCurve;
    }
    #endregion
    /*  #region Utility
      [Tooltip("Scales the bezier curve to changes in canvas size.")]
      [SerializeField, HideInInspector]
      private bool scaleToScreenCanvas;
      public bool ScaleToScreenCanvas
      {
          get => scaleToScreenCanvas;
          set 
          {
              scaleToScreenCanvas = value;
              if (WindowManager.instance)
              {
                  if(value == true)WindowManager.instance.onScreenSizeChange += OnScreenSizeChange;
                  else {WindowManager.instance.onScreenSizeChange -= OnScreenSizeChange; Debug.Log("unsubscribing");}
              }
              else 
              {
                  Debug.Log("No instance of Windowmanager found.");
              }
          } 
      }
      [SerializeField, HideInInspector]
      private Vector2 referenceScreenSize = new Vector2(1920, 1080);
      [SerializeField, HideInInspector]
      private Vector3 referencePositionLastUpdate;
      #endregion
    */
    [SerializeField, HideInInspector]
    Vector3 p0, p1, p2, p3;

    public Vector3 P0 { get => p0; set { p0 = value; Recalculate(); } }
    public Vector3 P1 { get => p1; set { p1 = value; Recalculate(); } }
    public Vector3 P2 { get => p2; set { p2 = value; Recalculate(); } }
    public Vector3 P3 { get => p3; set { p3 = value; Recalculate(); } }

    protected void Recalculate()
    {
        distanceLUT = BezierUtilities.GenerateDistanceLUT(new BezierUtilities.GetPoint(GetPoint), detail);
        pointsInCurve = new Vector3[detail + 1];
        pointsInCurveEased = new Vector3[detail + 1];
        maxMag = 0;
        for (int i = 0; i <= detail; i++)
        {
            float t = (float)i / detail;
            pointsInCurve[i] = GetPoint(t);
            pointsInCurveEased[i] = GetEasedPoint(t);

            if (i >= 1)
            {
                float segmentMagnitude = Mathf.Abs((pointsInCurveEased[i] - pointsInCurveEased[i - 1]).magnitude);
                maxMag = (segmentMagnitude > maxMag) ? segmentMagnitude : maxMag;
            }
        }
    }
    public Vector3 GetPoint(float t)
    {
        t = Mathf.Clamp01(t);

        Vector3 p0 = P0,
                p1 = P1,
                p2 = P2,
                p3 = P3;

        Vector3 a = Vector3.Lerp(p0, p1, t),
                b = Vector3.Lerp(p1, p2, t),
                c = Vector3.Lerp(p2, p3, t),
                d = Vector3.Lerp(a, b, t),
                e = Vector3.Lerp(b, c, t);

        Vector3 p = Vector3.Lerp(d, e, t);

        return Vector3.Lerp(d, e, t);
    }
    public Vector3 GetEasedPoint(float t)
    {
        t = Mathf.Clamp01(t);
        float normalizedTime = BezierUtilities.DistToT(distanceLUT, distanceLUT[distanceLUT.Length - 1] * t);
        float easedTime = PointSpacingFunction(0, 1, normalizedTime);
        Vector3 point = GetPoint(easedTime);
        return point;
    }
    /*
        #region  Screen Size Scaler
        private void OnEnable()
        {
            if (WindowManager.instance != null) { WindowManager.instance.onScreenSizeChange += OnScreenSizeChange; }
            // referencePositionLastUpdate = FindObjectOfType<Canvas>().transform.position;
        }
        private void OnDisable()
        {
            if (WindowManager.instance != null) { WindowManager.instance.onScreenSizeChange -= OnScreenSizeChange; }
        }
        private void OnScreenSizeChange(int width, int height)
        {
            if (referenceScreenSize.x != width || referenceScreenSize.y != height)
            {
                ScaleToScreenSize(width, height);
                Recalculate();
            }
        }
        //fix: set canvas reference somewhere else;
        private void ScaleToScreenSize(int width, int height)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            Vector3 referencePosition = canvas.transform.position;

            float xMod = (float)width / referenceScreenSize.x, yMod = (float)height / referenceScreenSize.y;
            Vector3 p0D = p0 - referencePositionLastUpdate;
            Vector3 p1D = p1 - referencePositionLastUpdate;
            Vector3 p2D = p2 - referencePositionLastUpdate;
            Vector3 p3D = p3 - referencePositionLastUpdate;

            p0 = referencePosition + new Vector3(p0D.x * xMod, p0D.y * yMod);
            p1 = referencePosition + new Vector3(p1D.x * xMod, p1D.y * yMod);
            p2 = referencePosition + new Vector3(p2D.x * xMod, p2D.y * yMod);
            p3 = referencePosition + new Vector3(p3D.x * xMod, p3D.y * yMod);

            referenceScreenSize = new Vector2(width, height);
            referencePositionLastUpdate = referencePosition;
        }

        #endregion
    #if UNITY_EDITOR
        public void DrawHandles()
        {
            Vector3[] points = { P0, P1, P2, P3 };
            Handles.DrawAAPolyLine(points[0..2]);
            Handles.DrawAAPolyLine(points[2..4]);

            if (pointsInCurveEased.Length > 0 && draw)
            {
                for (int i = 1; i < pointsInCurveEased.Length; i++)
                {
                    float segmentMag = Mathf.Abs((pointsInCurveEased[i] - pointsInCurveEased[i - 1]).magnitude);
                    Gizmos.color = Handles.color = Color.Lerp(Color.yellow, Color.magenta, segmentMag / maxMag);
                    Gizmos.DrawLine(pointsInCurveEased[i], pointsInCurveEased[i - 1]);
                    Handles.DrawWireDisc(pointsInCurveEased[i], Vector3.forward, HandleUtility.GetHandleSize(pointsInCurveEased[i]) * 0.04f);
                }
            }
        }

        private void OnDrawGizmos()
        {
            DrawHandles();
        }
    #endif
    */
}
