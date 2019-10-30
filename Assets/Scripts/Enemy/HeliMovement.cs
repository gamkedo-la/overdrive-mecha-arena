using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliMovement : MonoBehaviour
{
    private float altitudeChangeSpeed = 2.4f;
    private float altChangeRange = 8.0f;
    private float hoverHeightTarget;

    private float minAlt, maxAlt; // calculated at start from position +/- altChangeRange

    // Start is called before the first frame update
    void Start()
    {
        minAlt = transform.position.y - altChangeRange;
        maxAlt = transform.position.y + altChangeRange;
        hoverHeightTarget = transform.position.y;
        StartCoroutine(AIChangeMind());
    }

    IEnumerator AIChangeMind()
    {
        while(true)
        {
            float minMove = Random.Range(0.4f, 1.0f) * 7.0f;
            if(Random.Range(0.0f,1.0f)<0.5f)
            {
                minMove *= -1.0f;
            }
            hoverHeightTarget += minMove;
            hoverHeightTarget = Mathf.Clamp(hoverHeightTarget, minAlt, maxAlt);

            yield return new WaitForSeconds( Random.Range(0.4f,1.5f) );
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position-Vector3.up* altChangeRange, transform.position + Vector3.up * altChangeRange);
    }

    // Update is called once per frame
    void Update()
    {
        if(hoverHeightTarget < transform.position.y - altitudeChangeSpeed)
        {
            transform.position -= Vector3.up * altitudeChangeSpeed * Time.deltaTime;
        } else if (hoverHeightTarget > transform.position.y + altitudeChangeSpeed)
        {
            transform.position += Vector3.up * altitudeChangeSpeed * Time.deltaTime;
        }
    }
}
