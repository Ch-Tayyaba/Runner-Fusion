using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondAnim : MonoBehaviour
{
    public float rotationSpeed = 80f; // Rotation speed
    public float floatSpeed = 2f; // Speed of floating effect
    public float floatHeight = 0.3f; // Height of floating effect
    public float shineSpeed = 2f; // Speed of shining effect

    private Vector3 startPosition;
    private Renderer diamondRenderer;
    private Color originalColor;

    void Start()
    {
        startPosition = transform.position; // Store initial position
        diamondRenderer = GetComponent<Renderer>(); // Get the Renderer component
        originalColor = diamondRenderer.material.color; // Store the original color
    }

    void Update()
    {
        // Rotate the diamond
        transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);

        // Floating effect (moving up and down)
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Shining effect (change color intensity over time)
        float glow = Mathf.Abs(Mathf.Sin(Time.time * shineSpeed)) * 0.5f + 0.5f; // Creates a smooth pulsing effect
        diamondRenderer.material.color = originalColor * glow;
    }
}
