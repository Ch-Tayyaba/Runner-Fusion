using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMap : MonoBehaviour
{
    public GameObject Map;
    private float nextSpawnZ = 3000f; // Starting Z position for the first new map

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("NewMap"))
        {
            Instantiate(Map, new Vector3(0, 0, nextSpawnZ), Quaternion.identity);
            nextSpawnZ += 3000f; // Increment the Z position for the next spawn
        }

        if(other.gameObject.CompareTag("DelMap"))
        {
            //Destroy(other.transform.root.gameObject);

            
                GameObject delMapParent = other.transform.root.gameObject;

                if (delMapParent.CompareTag("Map")) // Ensure only maps are destroyed
                {
                    Destroy(delMapParent);
                    Debug.Log($"Destroyed DelMap parent: {delMapParent.name}");
                }
                else
                {
                    Debug.LogWarning($"Tried to destroy {delMapParent.name}, but it's not a map.");
                }
            
        }
    }
}
