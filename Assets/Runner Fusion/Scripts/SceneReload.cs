using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReload : MonoBehaviour
{

    public float sceneDuration = 30f; // Adjust duration if needed
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        // Condition to reload scene (e.g., time-based or level completion)
        if (timer >= sceneDuration || CheckCompletionCondition())
        {
            ReloadScene();
        }
    }

    // Function to reload the scene
    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    // Customize this function to define when the scene should restart
    bool CheckCompletionCondition()
    {
        // Example condition: if all enemies are destroyed or level is completed
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }
}
