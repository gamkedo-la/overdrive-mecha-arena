using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAggro : MonoBehaviour
{
    public event Action<Transform> Aggroed = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        var opponent = other.GetComponent<Health>();
        if (opponent != null)
        {
            Aggroed(opponent.transform);
            //Debug.Log("Enemy Attacking: " + opponent.name);
        }
    }
}
