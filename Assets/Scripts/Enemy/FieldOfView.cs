using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float viewRadius;
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    public float _viewRadius { get { return viewRadius; } }
    public float _viewAngle { get { return viewAngle; } }

    private void FindVisibleTargets()
    {
        Collider[] possibleTgts = Physics.OverlapSphere(transform.position + Vector3.up * 18.0f, viewRadius, targetMask);

        for (int i = 0; i < possibleTgts.Length; i++)
        {
            Transform tgt = possibleTgts[i].transform;
            Vector3 dirToTgt = (transform.position - tgt.position).normalized;
        }
    }

    public Vector3 DirFromAng(float angleDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
}
