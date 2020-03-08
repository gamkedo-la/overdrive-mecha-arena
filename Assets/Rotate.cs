using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float xAng;
    [SerializeField] private float yAng;
    [SerializeField] private float zAng;

    void Update()
    {
        //transform.Rotate(Vector3.up, 50 * Time.deltaTime);
        transform.Rotate(xAng * Time.deltaTime, yAng * Time.deltaTime, zAng * Time.deltaTime);
    }
}
