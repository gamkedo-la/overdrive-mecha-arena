using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    [SerializeField] private float fowardMoveSpeed = 50.0f;
    [SerializeField] private float turnSpeed = 100.0f;
    [SerializeField] private float backwardMoveSpeed = 35.0f;

    [SerializeField] private float strafingRightMoveSpeed = 50.0f;
    [SerializeField] private float strafingLeftMoveSpeed = 50.0f;

    [SerializeField] private float dashSpeed = 2.0f;
    [SerializeField] private float dashTimeLimit = 5.0f;

    [SerializeField] private Mecha mech;

    private PlayerShooting playerShooting;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        animator = GetComponentInChildren<Animator>();

        playerShooting = GetComponent<PlayerShooting>();

        //TODO: Make the following line toggable in game
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        fowardMoveSpeed = mech.fowardMoveSpeed;
        turnSpeed = mech.turnSpeed;
        backwardMoveSpeed = mech.backwardMoveSpeed;
        strafingLeftMoveSpeed = mech.strafingLeftMoveSpeed;
        strafingRightMoveSpeed = mech.strafingRightMoveSpeed;
        dashSpeed = mech.dashSpeed;
        dashTimeLimit = mech.dashTimeLimit;
    }

    private void Update()
    {
        var horizontalRot = Input.GetAxis("Mouse X");
        var horizontalStrafe = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var movement = new Vector3(horizontalRot, 0, vertical);

        animator.SetFloat("Speed", vertical);

        transform.Rotate(Vector3.up, horizontalRot * turnSpeed * Time.deltaTime);

        if (vertical != 0)
        {
            float moveSpeedToUse = vertical > 0 ? fowardMoveSpeed : backwardMoveSpeed;

            playerShooting.isTryingToDash = Input.GetButton("Dash");

            if (playerShooting.isTryingToDash)
            {
                characterController.SimpleMove(transform.forward * (moveSpeedToUse * dashSpeed) * vertical);
            }
            else
            {
                characterController.SimpleMove(transform.forward * moveSpeedToUse * vertical);
            }
        }
        if (horizontalStrafe != 0)
        {
            float strafeSpeedToUse = horizontalStrafe > 0 ? strafingRightMoveSpeed : strafingLeftMoveSpeed;

            playerShooting.isTryingToDash = Input.GetButton("Dash");

            if (playerShooting.isTryingToDash)
            {
                characterController.SimpleMove(transform.right * (strafeSpeedToUse * dashSpeed) * horizontalStrafe);
            }
            else
            {
                characterController.SimpleMove(transform.right * strafeSpeedToUse * horizontalStrafe);
            }
        }
    }
}
