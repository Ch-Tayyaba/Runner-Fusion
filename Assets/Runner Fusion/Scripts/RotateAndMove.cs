using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndMove : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation
    public float moveSpeed = 5f;       // Speed of movement
    //public Transform player;           // Assign the player object in the Inspector
    public float activationDistance = 5f; // Distance at which the drum starts moving

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void FixedUpdate()
    {
        // Check if the player is within activationDistance
        //if (Vector3.Distance(transform.position, player.position) <= activationDistance)
        {
            // Apply torque to rotate around the X-axis (rolling effect)
            rb.AddTorque(transform.up * rotationSpeed * Time.fixedDeltaTime, ForceMode.Force);

            // Apply force to move forward along its own axis
            rb.AddForce(-transform.forward * moveSpeed, ForceMode.Force);
        }
    }
}
