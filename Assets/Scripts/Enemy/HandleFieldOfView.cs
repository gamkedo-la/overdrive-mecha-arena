using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleFieldOfView : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private Transform mech;
    [SerializeField] private float fieldOfViewAngle = 100.0f;
    [SerializeField] public float visionRange = 100.0f;
    [SerializeField] private bool targetFound = false;
    private float rotationSpeed = 1.0f;

    private float rotationRate = 0;

    void Start()
    {
        mech = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(mech.position, Vector3.up, rotationSpeed * Time.deltaTime);

        //Vector3 targetPt = parent.position + Vector3.up * 18.0f; // projecting off feet
        //Vector3 direction = targetPt - transform.position; // think this as a 3D compass that points to this AI's target
        //float angle = Vector3.Angle(direction, transform.forward); // angle is important to test whether mech is in field of view
        //Debug.Log(angle);

        //if (angle >= fieldOfViewAngle || angle <= fieldOfViewAngle)
        //{
        //    rotationSpeed = -rotationSpeed;
        //}

        rotationRate += rotationSpeed * Time.deltaTime;

        float x = Mathf.Cos(rotationRate) * visionRange;
        float y = 0;
        float z = Mathf.Sin(rotationRate) * visionRange;

        transform.position = new Vector3(x, y, z) + (parent.position + Vector3.up * 18f);
    }
}
