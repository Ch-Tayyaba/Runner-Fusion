using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class WinFail : MonoBehaviour
{
    public TMP_Text coinText; // Use TMP_Text instead of Text
    public TMP_Text diamondText;
    public Slider power;

    public Slider magnetSlider; // Assign this in the Unity Inspector

    private int coinCount = 0;
    private int diamondCount = 0;
    private int powerCount = 0;

    public float magnetDuration = 5f;
    private bool isMagnetActive = false;
    private float leftLaneX = -200f;  // Adjusted based on your logs
    private float rightLaneX = 200f; // Adjusted based on your logs
    private float tolerance = 30f;  // Increased tolerance
    private float destroyRange = 500f; // Increased destroy range

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collided");
        if (other.CompareTag("coin")) // Check if collided object has "Coin" tag
        {
            Debug.Log("Coin Collected!");
            //Destroy(other.gameObject); // Destroy the coin
            coinCount++; // Increment the coin count
            UpdateUI(); // Update UI text

        }
        if (other.CompareTag("diamond")) 
        {
            Debug.Log("Diamond Collected!");
            //Destroy(other.gameObject); // Destroy
            diamondCount++; // Increment 
            UpdateUI(); // Update UI text

        }

        

        if(other.CompareTag("power"))
        {
            Debug.Log("Get Power");
            Destroy(other.gameObject); // Destroy the coin
            powerCount++; // Increment the coin count
            UpdateUI(); // Update UI text
            
        }


        //if (other.CompareTag("magnet"))
        //{
        //    Debug.Log("Get near coints");
        //    // Destroy all the gameobjects from left and right whith the tag "coin"
        //    coinCount++; // Increment the coin count
        //    UpdateUI(); // Update UI text

        //}

        if (other.CompareTag("magnet") && !isMagnetActive)
        {
            Debug.Log("Magnet Activated!");
            //StartCoroutine(ActivateMagnetEffect());
            Destroy(other.gameObject);
        }

    }

    IEnumerator ActivateMagnetEffect()
    {
        isMagnetActive = true;
        magnetSlider.gameObject.SetActive(true);
        magnetSlider.maxValue = magnetDuration;
        magnetSlider.value = magnetDuration;

        float elapsedTime = 0f;

        while (elapsedTime < magnetDuration)
        {
            magnetSlider.value = magnetDuration - elapsedTime;
            elapsedTime += Time.deltaTime;
            yield return null;
            Debug.Log("Magnet Active: Destroying Coins...");
            DestroyLeftRightCoins();
        }

        magnetSlider.gameObject.SetActive(false);
        isMagnetActive = false;
    }

    void DestroyLeftRightCoins()
    {
        float playerZ = transform.position.z; // Player's current Z position
        GameObject[] coins = GameObject.FindGameObjectsWithTag("coin"); // Get all coins in scene

        Debug.Log("Total Coins Found: " + coins.Length);

        foreach (GameObject coin in coins)
        {
            float coinX = coin.transform.position.x;
            float coinZ = coin.transform.position.z;

            bool isLeft = Mathf.Abs(coinX - leftLaneX) < tolerance;
            bool isRight = Mathf.Abs(coinX - rightLaneX) < tolerance;
            bool isWithinRange = Mathf.Abs(coinZ - playerZ) < destroyRange;

            Debug.Log($"Coin Position: ({coinX}, {coinZ}) | Left: {isLeft} | Right: {isRight} | In Range: {isWithinRange}");

            if ((isLeft || isRight) && isWithinRange)
            {
                Debug.Log("Destroying Coin at: " + coin.transform.position);
                Destroy(coin);
                coinCount++;
            }
        }
    }





    void UpdateUI()
    {
        coinText.text = coinCount.ToString();
        diamondText.text = diamondCount.ToString();
        power.value = power.value + powerCount; 

    }



    void OnDrawGizmos()
    {
        if (isMagnetActive)
        {
            Gizmos.color = Color.red;
            Vector3 playerPos = transform.position;

            // Draw left lane detection area
            Gizmos.DrawWireCube(new Vector3(leftLaneX, playerPos.y, playerPos.z), new Vector3(2f, 2f, destroyRange * 2));

            // Draw right lane detection area
            Gizmos.DrawWireCube(new Vector3(rightLaneX, playerPos.y, playerPos.z), new Vector3(2f, 2f, destroyRange * 2));

            // Draw player position
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(playerPos, 1f);
        }
    }

}
