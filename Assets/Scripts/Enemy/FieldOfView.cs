using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private Mecha mech;

    private float viewRadius;
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float searchDelay = 1.0f;

    private AICharacter fsm;

    public float _viewRadius { get { return viewRadius; } }
    public float _viewAngle { get { return viewAngle; } }

    private void Start()
    {
        fsm = GetComponent<AICharacter>();

        viewRadius = mech.range;
        StartCoroutine("SearchForTgts", searchDelay);
    }

    IEnumerator SearchForTgts(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        Collider[] possibleTgts = Physics.OverlapSphere(transform.position + Vector3.up * 18.0f, viewRadius, targetMask);

        for (int i = 0; i < possibleTgts.Length; i++)
        {
            Transform tgt = possibleTgts[i].transform;
            Vector3 dirToTgt = (transform.position - tgt.position).normalized;

            if(Vector3.Angle(transform.forward, dirToTgt) < viewAngle/2)
            {
                float distToTgt = Vector3.Distance(transform.position, tgt.position);
                if(!Physics.Raycast((transform.position + (Vector3.up * 18.0f) + (Vector3.forward * 8.0f)), dirToTgt, distToTgt, obstacleMask))
                {
                    Debug.Log(gameObject.name + " sees " + tgt.name);
                }
            }
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
