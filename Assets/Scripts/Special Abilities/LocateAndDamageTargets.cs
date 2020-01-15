﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LocateAndDamageTargets : MonoBehaviour
{
    [SerializeField] private float rotationalForce = 25.0f;
    [SerializeField] private float speed = 70.0f;
    [SerializeField] private int damage = 20;
    [SerializeField] private float secondsToWaitBeforeHoming = 2.5f;

    [SerializeField] private float blastOffForce = 3.5f;

    private bool shouldHomeIn = false;
    private Transform target;
    [SerializeField] private Transform myParent;
    private Rigidbody rb;
    private List<Transform> targets = new List<Transform>();

    private void Awake()
    {
        ///<summary>
        /// myParent = the mecha GO that contains all the logic 
        /// I.E. parent 1 = Spawn Point, parent 2 = Spawn Points List
        /// parent 3 = the mecha GO with the main scripts
        ///</summary>
        myParent = transform.parent.parent.parent.transform;

        List<GameObject> GOs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        foreach (GameObject item in GOs)
        {
            if(item != myParent.gameObject)
                targets.Add(item.transform);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != myParent.gameObject)
        {
            targets.Add(player.transform);
        }

        //Debug.Log(targets.Contains(myParent));
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        target = targets[Random.Range(0, targets.Count - 1)];

        StartCoroutine(waitBeforeHoming());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shouldHomeIn)
        {
            if (targets != null)
            {
                Vector3 direction = target.position - rb.position;
                direction.Normalize();
                Vector3 rotationAmount = Vector3.Cross(transform.forward, direction);
                rb.angularVelocity = rotationAmount * rotationalForce;
                rb.velocity = transform.forward * speed;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health tgtHealth = collision.collider.GetComponent<Health>();

        if(tgtHealth != null && collision.collider.gameObject != myParent.gameObject)
        {
            tgtHealth.TakeDamage(damage, gameObject.transform);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator waitBeforeHoming()
    {
        rb.AddForce(Random.Range(0.0f, blastOffForce), Random.Range(0.0f, blastOffForce), Random.Range(0.0f, blastOffForce));
        yield return new WaitForSeconds(secondsToWaitBeforeHoming);
        shouldHomeIn = true;
    }
}
