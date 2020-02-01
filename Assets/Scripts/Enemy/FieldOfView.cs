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

    private EnemyShooting shootingScript;
    private AICharacter fsm;

    public float _viewRadius { get { return viewRadius; } }
    public float _viewAngle { get { return viewAngle; } }

    private void Start()
    {
        shootingScript = GetComponent<EnemyShooting>();
        fsm = GetComponent<AICharacter>();

        viewRadius = mech.range;

        StartCoroutine("SearchForTgts", searchDelay);
    }

    IEnumerator SearchForTgts(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        if (fsm._currentState.StateName() != "chase state")
        {
            Collider[] possibleTgts = Physics.OverlapSphere(transform.position + Vector3.up * 18.0f, viewRadius, targetMask, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < possibleTgts.Length; i++)
            {
                Transform tgt = possibleTgts[i].transform;
                if (tgt.root != transform)
                {
                    Vector3 dirToTgt = (transform.position - tgt.position).normalized;

                    if (Vector3.Angle(transform.forward, dirToTgt) < viewAngle / 2)
                    {
                        float distToTgt = Vector3.Distance(transform.position, tgt.position);
                        if (!Physics.Raycast((transform.position + (Vector3.up * 18.0f)), dirToTgt, distToTgt, obstacleMask, QueryTriggerInteraction.Ignore))
                        {
                            //Debug.Log(gameObject.name + " sees " + tgt.name);
                            Health validTgt = tgt.GetComponent<Health>();

                            if (validTgt != null && fsm.getValidTargets.Contains(validTgt) == false)
                            {
                                fsm.AddTgtToSuperList(validTgt);
                                shootingScript._hasLostTgt = false;
                                fsm.SetChaseStateViaFieldOfView();
                            }
                            else if (validTgt != null)
                            {
                                shootingScript._hasLostTgt = false;
                                fsm.SetChaseStateViaFieldOfView();
                            }
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
        }
    }

    public Vector3 DirFromAng(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
}
