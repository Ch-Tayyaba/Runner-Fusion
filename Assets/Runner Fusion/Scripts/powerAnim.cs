using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerAnim : MonoBehaviour
{

    public float rotationSpeed = -100f; // Negative for opposite rotation
    public float pulseSpeed = 2f; // Speed of pulsing effect
    public float pulseScale = 0.2f; // How much the size changes
    public float floatSpeed = 2f; // Speed of floating effect
    public float floatHeight = 0.3f; // Height of floating effect

    private Vector3 originalScale;
    private Vector3 startPosition;

    void Start()
    {
        originalScale = transform.localScale; // Store the original size
        startPosition = transform.position;   // Store the starting position
    }

    void Update()
    {
        // Rotate in the opposite direction
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Pulsating effect (scaling up and down)
        float scaleFactor = Mathf.Sin(Time.time * pulseSpeed) * pulseScale + 1;
        transform.localScale = originalScale * scaleFactor;

        // Floating effect (moving up and down)
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
