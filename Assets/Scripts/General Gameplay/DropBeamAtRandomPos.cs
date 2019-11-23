using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBeamAtRandomPos : MonoBehaviour
{
    private GameObject beamPrefab;

    [SerializeField] private float beamDropTimer = 30.0f;
    private float dropCount;
    private bool canDropBeam = false;

    void Awake()
    {
        beamPrefab = Resources.Load("Beam") as GameObject;
    }

    private void Start()
    {
        //Debug.Log(beamPrefab);
    }

    private void Update()
    {
        dropCount += Time.deltaTime;
        if(dropCount >= beamDropTimer)
        {
            canDropBeam = true;
            dropCount = 0f;
        }
        else
        {
            canDropBeam = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //get random vector3 for beam's spawn location
        //ensure y pos is roughly the same as the trigger zone's height
        //spawn prefab at location within trigger zone
        if (canDropBeam)
        {
            Instantiate(beamPrefab);
        }
        else
        {
            //Debug.Log("Cannot drop a beam!");
        }
    }
}
