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
    public GameObject PPV;
    private IEnumerator coroutine;

    private GameObject audioManager;
    PlayerFMODEvents playerEventsScript;
    bool start = true;

    private bool usingGamepad = false;
    private DoubleStatsSpecial doubleStats;

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

        PPV = GameObject.FindGameObjectWithTag("PostProcessing");
        PPV.SetActive(false);

        playerEventsScript = gameObject.GetComponent<PlayerFMODEvents>();
        if(GetComponent<DoubleStatsSpecial>() != null)
        {
            doubleStats = GetComponent<DoubleStatsSpecial>();
        }
    }

    private void Update()
    {
        var horizontalRot = 0.0f;
        var horizontalStrafe = 0.0f;
        var vertical = 0.0f;
        if (!usingGamepad)
        {
            horizontalRot = Input.GetAxis("Mouse X");
            horizontalStrafe = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
        else
        {
            horizontalRot = Input.GetAxis("JoyRotX");
            horizontalStrafe = Input.GetAxis("HorizontalJoy");
            vertical = Input.GetAxis("VerticalJoy");
            //Debug.Log(Input.GetAxis("HorizontalJoy"));
        }

        if(doubleStats != null && doubleStats._areStatsBuffed)
        {
            fowardMoveSpeed = mech.fowardMoveSpeed * 2;
            turnSpeed = mech.turnSpeed * 2;
            backwardMoveSpeed = mech.backwardMoveSpeed * 2;
            strafingLeftMoveSpeed = mech.strafingLeftMoveSpeed * 2;
            strafingRightMoveSpeed = mech.strafingRightMoveSpeed * 2;
            dashSpeed = mech.dashSpeed * 2;
            dashTimeLimit = mech.dashTimeLimit * 2;
        }
        else
        {
            fowardMoveSpeed = mech.fowardMoveSpeed;
            turnSpeed = mech.turnSpeed;
            backwardMoveSpeed = mech.backwardMoveSpeed;
            strafingLeftMoveSpeed = mech.strafingLeftMoveSpeed;
            strafingRightMoveSpeed = mech.strafingRightMoveSpeed;
            dashSpeed = mech.dashSpeed;
            dashTimeLimit = mech.dashTimeLimit;
        }

        var movement = new Vector3(horizontalRot, 0, vertical);

        transform.Rotate(Vector3.up, horizontalRot * turnSpeed * Time.deltaTime);

        playerShooting.isTryingToDash = Input.GetButton("Dash") && (vertical != 0 || horizontalStrafe != 0);

        if (playerShooting.isTryingToDash)
        {
            PPV.SetActive(true);
        }
        else
        {
            PPV.SetActive(false);
        }
        Vector3 moveVect = Vector3.zero;

        Vector3 groundFwd = transform.forward;

        if (vertical != 0)
        {
            float moveSpeedToUse = vertical > 0 ? fowardMoveSpeed : backwardMoveSpeed;

            if (playerShooting.isTryingToDash)
            {
                moveVect += (groundFwd * (moveSpeedToUse * dashSpeed) * vertical);
            }
            else
            {
                moveVect += (groundFwd * moveSpeedToUse * vertical);
            }
        }
        if (horizontalStrafe != 0)
        {
            float strafeSpeedToUse = horizontalStrafe > 0 ? strafingRightMoveSpeed : strafingLeftMoveSpeed;

            if (playerShooting.isTryingToDash)
            {
                moveVect += (transform.right * (strafeSpeedToUse * dashSpeed) * horizontalStrafe);
            }
            else
            {
                moveVect += (transform.right * strafeSpeedToUse * horizontalStrafe);
            }
        }

        characterController.SimpleMove(moveVect);

        //Audio related stuff
        if (vertical != 0 || horizontalStrafe != 0 || horizontalRot != 0)
        {
            playerEventsScript.PlayMechaMovementSound();

            if (Input.GetButtonDown("Dash"))
            {
                playerEventsScript.PlayDashSound();
            }
        }
        else if (vertical == 0 && horizontalStrafe == 0 && horizontalRot == 0)
        {
            playerEventsScript.StopMechaMovementSound();
        }

        if (Input.GetButtonUp("Dash"))
        {
            playerEventsScript.StopDashSound();
        }

    }
}
