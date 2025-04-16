using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnim : MonoBehaviour
{
    public float moveDistance = 1f; // How far the arrow moves
    public float moveSpeed = 2f; // Speed of movement

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Store initial position
    }

    void Update()
    {
        // Move forward and backward using Mathf.PingPong
        float newZ = startPosition.z + Mathf.PingPong(Time.time * moveSpeed, moveDistance) - (moveDistance / 2);
        transform.position = new Vector3(startPosition.x, startPosition.y, newZ);
    }
}
