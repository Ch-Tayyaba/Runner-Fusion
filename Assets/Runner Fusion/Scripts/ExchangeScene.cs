using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExchangeScene : MonoBehaviour
{
    public void Home()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void Play()
    {
        SceneManager.LoadSceneAsync(1);
    }


    public void PlayAgain()
    {
        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        AsyncOperation asyncUnload = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        asyncUnload.allowSceneActivation = false; // Prevent automatic activation

        yield return new WaitForSeconds(0.1f); // Small delay for unloading assets

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        asyncUnload.allowSceneActivation = true; // Now activate the scene
    }



}
