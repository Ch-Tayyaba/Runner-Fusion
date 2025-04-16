using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPathManager : MonoBehaviour
{
    //public BezierCurve[] bezierCurves; // Array of multiple Bezier curves
    //public int curveResolution = 100;  // Resolution for each curve
    //public float offsetDistance = 40f; // Distance for left and right curves

    //private List<Vector3> fullPathPoints = new List<Vector3>();
    //private List<Vector3> leftPathPoints = new List<Vector3>();
    //private List<Vector3> rightPathPoints = new List<Vector3>();

    //void Start()
    //{
    //    GenerateFullPath();
    //}

    //private void GenerateFullPath()
    //{
    //    fullPathPoints.Clear();
    //    leftPathPoints.Clear();
    //    rightPathPoints.Clear();

    //    foreach (BezierCurve curve in bezierCurves)
    //    {
    //        Vector3[] curvePoints = curve.GetBezierPoints();
    //        fullPathPoints.AddRange(curvePoints);

    //        for (int i = 0; i < curvePoints.Length - 1; i++)
    //        {
    //            Vector3 direction = (curvePoints[i + 1] - curvePoints[i]).normalized;
    //            Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;

    //            leftPathPoints.Add(curvePoints[i] + (perpendicular * offsetDistance));
    //            rightPathPoints.Add(curvePoints[i] - (perpendicular * offsetDistance));
    //        }
    //    }
    //}

    //public BezierCurve[] bezierCurves; // Array of multiple Bezier curves
    //public int curveResolution = 100;  // Resolution for each curve
    //public float offsetDistance = 40f; // Distance for left and right curves
    //public Vector3 repeatOffset = new Vector3(-240, 0, 0); // Offset for repeating path

    //private List<Vector3> fullPathPoints = new List<Vector3>();
    //private List<Vector3> leftPathPoints = new List<Vector3>();
    //private List<Vector3> rightPathPoints = new List<Vector3>();

    ////public List<Vector3> GetFullPath() => new List<Vector3>(fullPathPoints); // Expose the path

    //void Start()
    //{
    //    GenerateFullPath(Vector3.zero); // Generate first path at default position
    //}

    //public void GenerateFullPath(Vector3 offset)
    //{
    //    fullPathPoints.Clear();
    //    leftPathPoints.Clear();
    //    rightPathPoints.Clear();

    //    foreach (BezierCurve curve in bezierCurves)
    //    {
    //        Vector3[] curvePoints = curve.GetBezierPoints();
    //        for (int i = 0; i < curvePoints.Length; i++)
    //        {
    //            fullPathPoints.Add(curvePoints[i] + offset);
    //        }

    //        for (int i = 0; i < curvePoints.Length - 1; i++)
    //        {
    //            Vector3 direction = (curvePoints[i + 1] - curvePoints[i]).normalized;
    //            Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;

    //            leftPathPoints.Add(curvePoints[i] + (perpendicular * offsetDistance) + offset);
    //            rightPathPoints.Add(curvePoints[i] - (perpendicular * offsetDistance) + offset);
    //        }
    //    }
    //}

    //public void CreateNextPath()
    //{
    //    Vector3 lastPoint = fullPathPoints[fullPathPoints.Count - 1]; // Get last path position
    //    GenerateFullPath(lastPoint + repeatOffset); // Create new path ahead
    //}

    //public Vector3[] GetFullPath() => fullPathPoints.ToArray();
    //public Vector3[] GetLeftPath() => leftPathPoints.ToArray();
    //public Vector3[] GetRightPath() => rightPathPoints.ToArray();








    public BezierCurve[] bezierCurves; // Array of multiple Bezier curves
    public int curveResolution = 100;  // Resolution for each curve
    public float offsetDistance = 40f; // Distance for left and right curves
    public Vector3 repeatOffset = new Vector3(-240, 0, 0); // Offset for repeating path

    private List<Vector3> fullPathPoints = new List<Vector3>();
    private List<Vector3> leftPathPoints = new List<Vector3>();
    private List<Vector3> rightPathPoints = new List<Vector3>();

    void Awake()
    {
        if (bezierCurves == null || bezierCurves.Length == 0)
        {
            Debug.LogError("Bezier curves are not assigned in BezierPathManager.");
            return;
        }
        GenerateFullPath(Vector3.zero);
    }

    public void GenerateFullPath(Vector3 offset)
    {
        fullPathPoints.Clear();
        leftPathPoints.Clear();
        rightPathPoints.Clear();

        foreach (BezierCurve curve in bezierCurves)
        {
            if (curve == null)
            {
                Debug.LogError("A BezierCurve reference is missing in the bezierCurves array.");
                continue;
            }

            Vector3[] curvePoints = curve.GetBezierPoints();
            if (curvePoints == null || curvePoints.Length == 0)
            {
                Debug.LogError($"BezierCurve {curve.name} did not generate points.");
                continue;
            }

            for (int i = 0; i < curvePoints.Length; i++)
            {
                fullPathPoints.Add(curvePoints[i] + offset);
            }

            for (int i = 0; i < curvePoints.Length - 1; i++)
            {
                Vector3 direction = (curvePoints[i + 1] - curvePoints[i]).normalized;
                Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;

                leftPathPoints.Add(curvePoints[i] + (perpendicular * offsetDistance) + offset);
                rightPathPoints.Add(curvePoints[i] - (perpendicular * offsetDistance) + offset);
            }
        }

        if (fullPathPoints.Count == 0)
        {
            Debug.LogError("Full path generation failed. Check BezierCurve configurations.");
        }
    }

    public void CreateNextPath()
    {
        if (fullPathPoints == null || fullPathPoints.Count == 0)
        {
            Debug.LogError("Cannot create next path. FullPathPoints is empty.");
            return;
        }

        Vector3 lastPoint = fullPathPoints[fullPathPoints.Count - 1];
        GenerateFullPath(lastPoint + repeatOffset);
    }

    public Vector3[] GetFullPath() => fullPathPoints.ToArray();
    public Vector3[] GetLeftPath() => leftPathPoints.ToArray();
    public Vector3[] GetRightPath() => rightPathPoints.ToArray();












    private void OnDrawGizmos()
    {
        if (bezierCurves == null || bezierCurves.Length == 0) return;

        GenerateFullPath(Vector3.zero);

        Gizmos.color = Color.cyan; // Main path
        DrawPath(fullPathPoints);

        Gizmos.color = Color.red; // Left path
        DrawPath(leftPathPoints);

        Gizmos.color = Color.green; // Right path
        DrawPath(rightPathPoints);
    }

    private void DrawPath(List<Vector3> pathPoints)
    {
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]); // Draw curve connections
            Gizmos.DrawSphere(pathPoints[i], 0.1f); // Draw small spheres on points
        }
    }
}
