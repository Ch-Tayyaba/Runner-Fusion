using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowBezier : MonoBehaviour
{

    public BezierPathManager bezierPathManager; // Reference to BezierPathManager
    public float moveSpeed = 5f;
    public float laneDistance = 2f;
    public float laneSwitchSpeed = 8f;

    private Vector3[] pathPoints;
    private int currentIndex = 0;
    private int lane = 0;
    private Vector3 laneOffset;

    void Start()
    {
        pathPoints = bezierPathManager.GetFullPath();
        transform.position = pathPoints[0]; // Start at first point
    }

    void Update()
    {
        if (currentIndex < pathPoints.Length - 1)
        {
            // Move continuously along the path
            transform.position = Vector3.MoveTowards(transform.position, pathPoints[currentIndex] + laneOffset, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pathPoints[currentIndex] + laneOffset) < 0.1f)
            {
                currentIndex++;
            }
        }
        else
        {
            // If reached the end, restart path
            currentIndex = 0;
            transform.position = pathPoints[0];
        }

        // Handle left/right movement
        if (Input.GetKeyDown(KeyCode.LeftArrow) && lane > -1)
        {
            lane--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && lane < 1)
        {
            lane++;
        }

        // Smoothly adjust lane position
        Vector3 targetOffset = new Vector3(lane * laneDistance, 0, 0);
        laneOffset = Vector3.Lerp(laneOffset, targetOffset, laneSwitchSpeed * Time.deltaTime);
    }
}
