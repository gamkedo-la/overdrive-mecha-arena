using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchEMPBombs : MonoBehaviour
{
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;

    [SerializeField] private GameObject empBombPrefab;

    private void Update()
    {
        center = center + transform.localPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, .25f,0, 0.5f);
        Gizmos.DrawCube(transform.localPosition + center, size);
    }
}
