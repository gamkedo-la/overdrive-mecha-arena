using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchEMPBombs : MonoBehaviour
{
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;

    [SerializeField] private GameObject empBombPrefab;

    private float spawnTimer = 0.0f;
    [SerializeField] private float launchForce = 500f;
    [SerializeField] private float bombLaunchTime = 1.5f;

    private void Start()
    {
        center = center + transform.localPosition;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= bombLaunchTime)
        {
            LaunchEMP();
            spawnTimer = 0f;
        }
    }

    private void LaunchEMP()
    {
        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y/2, size.y / 2), size.z);

        GameObject bomb = Instantiate(empBombPrefab, pos, Quaternion.identity) as GameObject;
        bomb.GetComponent<Rigidbody>().AddForce(transform.forward * launchForce);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, .25f,0, 0.5f);
        Gizmos.DrawCube(transform.localPosition + center, size);
    }
}
