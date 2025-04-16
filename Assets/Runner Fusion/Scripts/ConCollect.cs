using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConCollect : MonoBehaviour
{
    public AudioClip coinSound;  // Assign sound in Inspector
    private AudioSource audioSource;

    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ensure the player has the "Player" tag
        {
            audioSource.PlayOneShot(coinSound); // Play sound
            Destroy(gameObject, 0.05f);  // Destroy coin after sound plays

            //gameObject.SetActive(false); // Disable coin when collected
            //Invoke("Respawn", 2f); // Respawn after 2 seconds
            
        }
    }

    
}
