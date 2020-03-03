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
        PlayerMovement movement = _other.GetComponent<PlayerMovement>();

        if (movement != null)
            followPlayer.target = movement.transform;
    }
}
