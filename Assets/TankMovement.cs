using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    private float driveSpeed = 2.4f;
    private float driveRange = 8.0f;
    private float linePercGoal = 0.5f;
    private Vector3 driveToPtTarget;
    public GameObject debugNodeVis;

    private Vector3 minPos, maxPos;

    // Start is called before the first frame update
    void Start()
    {
        minPos = transform.position - driveRange * transform.forward;
        maxPos = transform.position + driveRange * transform.forward;
        driveToPtTarget = transform.position;
        StartCoroutine(AIChangeMind());
    }

    private void updateDriveDest()
    {
        driveToPtTarget = Vector3.Lerp(minPos, maxPos, linePercGoal);
        debugNodeVis.transform.position = driveToPtTarget;
    }

    IEnumerator AIChangeMind()
    {
        while (true)
        {
            float minMove = Random.Range(0.4f, 1.0f) * 0.4f;
            if (Random.Range(0.0f, 1.0f) < 0.5f)
            {
                minMove *= -1.0f;
            }
            linePercGoal += minMove;
            linePercGoal = Mathf.Clamp01(linePercGoal);

            updateDriveDest();

            yield return new WaitForSeconds(Random.Range(0.4f, 1.5f));
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position - transform.forward * driveRange, transform.position + transform.forward * driveRange);
    }

    // Update is called once per frame
    void Update()
    {
        float speedDir;
        float angleToDriveGoal = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(driveToPtTarget - transform.position));
        Debug.Log(angleToDriveGoal);
        if ( Vector3.Distance(transform.position, driveToPtTarget) < driveSpeed)
        {
            speedDir = 0.0f;
        } else if (angleToDriveGoal < 90.0f)
        {
            speedDir = 1.0f;
        }
        else
        {
            speedDir = -1.0f;
        }
        transform.position += transform.forward * driveSpeed * Time.deltaTime * speedDir;
    }
}
