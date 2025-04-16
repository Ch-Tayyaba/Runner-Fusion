using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{

    public Transform startPoint;  // First point of the curve
    public Transform controlPoint; // Control point to bend the curve
    public Transform endPoint;    // Last point of the curve
    public Transform player; // Reference to the player

    public int curveResolution = 50; // How smooth the curve is

    private void Update()
    {

    }

    public Vector3[] GetBezierPoints()
    {
        Vector3[] bezierPoints = new Vector3[curveResolution];

        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)(curveResolution - 1);
            bezierPoints[i] = CalculateQuadraticBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position);
        }

        return bezierPoints;
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }

    private Vector3 GetBezierTangent(float t)
    {
        Vector3 p0 = startPoint.position;
        Vector3 p1 = controlPoint.position;
        Vector3 p2 = endPoint.position;

        // Derivative of the quadratic Bezier curve
        return (2 * (1 - t) * (p1 - p0)) + (2 * t * (p2 - p1));
    }

    private void OnDrawGizmos()
    {
        if (startPoint && controlPoint && endPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(startPoint.position, 0.2f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(controlPoint.position, 0.2f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(endPoint.position, 0.2f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(startPoint.position, controlPoint.position);
            Gizmos.DrawLine(controlPoint.position, endPoint.position);
        }
    }

    //public Transform startPoint;  // First point of the curve
    //public Transform controlPoint; // Control point to bend the curve
    //public Transform endPoint;    // Last point of the curve

    //public int curveResolution = 50; // How smooth the curve is

    //public Vector3[] GetBezierPoints()
    //{
    //    Vector3[] bezierPoints = new Vector3[curveResolution];

    //    for (int i = 0; i < curveResolution; i++)
    //    {
    //        float t = i / (float)(curveResolution - 1);
    //        bezierPoints[i] = CalculateQuadraticBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position);
    //    }

    //    return bezierPoints;
    //}

    //private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    //{
    //    float u = 1 - t;
    //    return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    //}



    //private void OnDrawGizmos()
    //{
    //    if (startPoint && controlPoint && endPoint)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawSphere(startPoint.position, 0.2f);
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawSphere(controlPoint.position, 0.2f);
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawSphere(endPoint.position, 0.2f);

    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawLine(startPoint.position, controlPoint.position);
    //        Gizmos.DrawLine(controlPoint.position, endPoint.position);
    //    }
    //}
}
