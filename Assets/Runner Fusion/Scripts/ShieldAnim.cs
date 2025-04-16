using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnim : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation
    public float floatSpeed = 1.5f; // Speed of floating effect
    public float floatHeight = 0.2f; // Height of floating effect
    public float pulseSpeed = 2.5f; // Speed of pulsing effect
    public float pulseScale = 0.1f; // Intensity of pulsing effect

    private Vector3 originalScale;
    private Vector3 startPosition;
    private Renderer shieldRenderer;
    private Color originalColor;

    void Start()
    {
        originalScale = transform.localScale; // Store initial scale
        startPosition = transform.position; // Store initial position
        shieldRenderer = GetComponent<Renderer>(); // Get the Renderer component
        originalColor = shieldRenderer.material.color; // Store original color
    }

    void Update()
    {
        // Rotate the shield smoothly
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Floating effect (moving up and down)
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Pulsating effect (Expanding and Shrinking)
        float scaleFactor = Mathf.Sin(Time.time * pulseSpeed) * pulseScale + 1;
        transform.localScale = originalScale * scaleFactor;

        // Glowing effect (Shield Energy Pulse)
        float glowIntensity = Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed)) * 0.5f + 0.5f;
        shieldRenderer.material.color = originalColor * glowIntensity;
    }
}
