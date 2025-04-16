using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Camera offset from the player

    void LateUpdate()
    {
        if (player != null) // Check if player reference exists
        {
            transform.position = player.position + offset;
        }
        else
        {
            Debug.LogWarning("Player reference is missing or destroyed.");
        }
    }
}
