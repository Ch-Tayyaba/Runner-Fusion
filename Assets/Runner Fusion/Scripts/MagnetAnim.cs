using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetAnim : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation
    public float floatSpeed = 2f; // Speed of floating effect
    public float floatHeight = 0.2f; // Height of floating effect
    public float pulseSpeed = 3f; // Speed of pulse effect
    public float pulseScale = 0.1f; // Intensity of pulse effect

    private Vector3 originalScale;
    private Vector3 startPosition;

    void Start()
    {
        originalScale = transform.localScale; // Store initial scale
        startPosition = transform.position;   // Store initial position
    }

    void Update()
    {
        // Rotate the magnet slightly for a dynamic effect
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Floating effect (moving up and down)
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Pulsating effect (scaling in and out)
        float scaleFactor = Mathf.Sin(Time.time * pulseSpeed) * pulseScale + 1;
        transform.localScale = originalScale * scaleFactor;
    }
}
