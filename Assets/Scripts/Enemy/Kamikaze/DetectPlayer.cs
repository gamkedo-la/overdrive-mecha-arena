using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private FollowPlayer followPlayer;

    private void Awake()
    {
        followPlayer = transform.parent.GetComponent<FollowPlayer>();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if(_other.CompareTag("Player") || _other.CompareTag("Enemy"))
        {
            followPlayer.target = _other.transform;
        }
    }
}
