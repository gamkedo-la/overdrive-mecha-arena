using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpawner : MonoBehaviour
{
    public GameObject[] prefabArrayToSpawn;

    void Update()
    {
        if(Input.anyKeyDown == false)
        {
            return; // skip checking if no keys used
        }

        int spawnIdx = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawnIdx = 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spawnIdx = 2;
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            spawnIdx = 3;
        } else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            spawnIdx = 4;
        } else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            spawnIdx = 5;
        } else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            spawnIdx = 6;
        } else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            spawnIdx = 7;
        } else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            spawnIdx = 8;
        } else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            spawnIdx = 9;
        } else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            spawnIdx = 10;
        }

        if(spawnIdx == -1) // no relevant key pressed, bail
        {
            return;
        }

        spawnIdx--; // -1 to zero index array, keyboard starts with 1, 2, 3...

        if (spawnIdx < prefabArrayToSpawn.Length)
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject newGO = GameObject.Instantiate(prefabArrayToSpawn[spawnIdx],
                    hitInfo.point + Vector3.up,
                    Quaternion.identity);
                Debug.Log("Debug spawned: " + newGO.name);
            }
            else {
                Debug.Log("debug spawn couldn't find a valid point to spawn");
            }
        } else
        {
            Debug.Log("prefabArrayToSpawn has no match for key " + (spawnIdx + 1)); //+1 to un-index
        }



    }
}
