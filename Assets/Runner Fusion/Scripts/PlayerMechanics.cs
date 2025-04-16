using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMechanics : MonoBehaviour
{
    public GameObject winPanel; // Assign your Win UI panel in the Inspector
    public GameObject blurPanel; // The Full-Screen Blur Panel
    public TMP_Text coin;
    public TMP_Text diamond;

    public TMP_Text finalcoin;
    public TMP_Text finaldiamond;
    public BezierPathManager bezierPathManager;
    public float moveSpeed = 5f;
    public float laneSpeed = 8f;
    public float laneDistance = 2f;

    private Animator animator;
    private Vector3 velocity;
    private Vector3[] pathPoints;
    private int currentIndex = 0;
    private int lane = 0;
    private float currentLaneOffset = 0f;
    private bool isRunning = false;

    private Rigidbody rb;
    private bool gameEnded = false;


    public float jumpSpeed = 2f;      // Controls jump speed
    public float jumpHeight = 2f;     // Controls jump peak height
    public float jumpDistance = 10f;  // Controls how far forward the jump goes

    public float jumpDuration = 1f; // Time to reach peak
    private bool isJumping = false;
    public float forwardDistance = 3f; // Adjust distance moved forward

    //public float jumpForce = 10f;  // Adjust jump force

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
        else
        {
            animator.Rebind(); // Ensure the animator is properly initialized
        }


        if (bezierPathManager == null)
        {
            Debug.LogError("BezierPathManager is not assigned in PlayerMechanics.");
            return;
        }

        pathPoints = bezierPathManager.GetFullPath();

        if (pathPoints == null || pathPoints.Length == 0)
        {
            Debug.LogError("Bezier path points are null or empty. Ensure paths are being generated correctly.");
            return;
        }

        transform.position = pathPoints[0];

        if (animator != null)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsJump", false);
            animator.SetBool("IsDamage", false);
        }
        else
        {
            Debug.LogError("Animator component is missing from the player.");
        }
    }

    void Update()
    {
        
        HandleLaneSwitching();
    }
    void FixedUpdate()
    {
        if (gameEnded || isJumping) // Skip movement while jumping
            return;

        if (pathPoints == null || pathPoints.Length == 0) return;

        HandleRunStart();




        if (isRunning && currentIndex < pathPoints.Length - 1)
        {
              moveSpeed += Time.deltaTime * 0.5f; // Adjust 0.5f to control speed growth rate
               moveSpeed = Mathf.Min(moveSpeed, 500f); // Set a maximum speed limit

            Vector3 targetPosition = pathPoints[currentIndex] + new Vector3(currentLaneOffset, 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            if ((transform.position - targetPosition).sqrMagnitude < 0.05f)
            {
                currentIndex++;
            }
        }

        if (currentIndex >= pathPoints.Length - 1)
        {
            bezierPathManager.CreateNextPath();
            pathPoints = bezierPathManager.GetFullPath();

            if (pathPoints == null || pathPoints.Length == 0)
            {
                Debug.LogError("Failed to retrieve new path after generating next segment.");
                return;
            }

            currentIndex = 0;
        }

        if (currentIndex >= pathPoints.Length - 1)
        {
            isRunning = false;
            if (animator != null)
            {
                Animator animator = GetComponent<Animator>();
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsDamage", false);
                animator.SetBool("IsJump", false);
            }
            currentIndex = 0;
        }

    }

    void HandleLaneSwitching()
    {
        // ?? Keyboard Input (Arrow Keys & A/D)
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (lane > -1) lane--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (lane < 1) lane++;
        }


        // Use Lerp for smooth lane switching
        currentLaneOffset = Mathf.Lerp(currentLaneOffset, lane * laneDistance, laneSpeed * Time.deltaTime);
    }


    void HandleRunStart()
    {
        // ?? Keyboard: Spacebar starts the run
        if (!isRunning && Input.GetKeyDown(KeyCode.Space))
        {
            StartRunning();
        }
    }

    void StartRunning()
    {
        isRunning = true;
        animator.SetBool("IsRunning", true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("blockage"))
        {
            Debug.Log("Game End");

            Rigidbody rbb = other.gameObject.GetComponent<Rigidbody>();
            if (rbb != null)
            {
                rbb.velocity = Vector3.zero; // Stop movement
                rbb.angularVelocity = Vector3.zero; // Stop rotation
                rbb.constraints = RigidbodyConstraints.FreezeAll; // Freeze position & rotation
            }

            StopPlayer();
        }
        if (other.gameObject.CompareTag("Arrow") && !isJumping)
        {
            
            Debug.Log("To Jump");
           
            StartCoroutine(Jump());
            
        }
        if(other.gameObject.CompareTag("DelMap"))
        {
            Destroy(gameObject);
        }

    }

    void StopPlayer()
    {
        // Set gameEnded flag to true so movement code won't run anymore
        gameEnded = true;

        animator.SetBool("IsDamage", true);
        //animator.SetTrigger("IsDamage");

        finalcoin.text = coin.text;
        finaldiamond.text = diamond.text;

        blurPanel.SetActive(true); // Enable blur effect
        winPanel.SetActive(true); // Show the Win panel
        
    }

   


    
    // **Helper function for smooth Bezier-like jump movement**
    private Vector3 BezierLerp(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }

    private IEnumerator Jump()

    {
        animator.SetBool("IsJump", true);
        isJumping = true;
        float elapsedTime = 0f;
        float totalDuration = 1f / jumpSpeed; // Speed affects duration

        // Maintain the lane offset during the jump
        Vector3 laneOffsetVector = new Vector3(currentLaneOffset, 0, 0);

        Vector3 startPos = transform.position;  // Keep player's exact position

        int startIndex = currentIndex;

        // Determine jump target with adjustable distance
        int targetIndex = Mathf.Min(currentIndex + Mathf.RoundToInt(jumpDistance), pathPoints.Length - 1);

        Vector3 endPos = pathPoints[targetIndex] + laneOffsetVector; // Ensure lane consistency
        Vector3 peakPos = (startPos + endPos) / 2 + Vector3.up * jumpHeight; // Adjust peak in the same lane

        // Smoothly move along the jump curve
        while (elapsedTime < totalDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / totalDuration);
            transform.position = BezierLerp(startPos, peakPos, endPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos; // Ensure exact landing
        currentIndex = targetIndex;
        isJumping = false;

        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

        animator.SetBool("IsJump", false);
    }




}
