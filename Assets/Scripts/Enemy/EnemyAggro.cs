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
        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Aggroed(player.transform);
            //Debug.Log("Enemy Attacking");
        }
    }
}
