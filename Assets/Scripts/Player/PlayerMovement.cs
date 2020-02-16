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

        var movement = new Vector3(horizontalRot, 0, vertical);

        animator.SetFloat("Speed", vertical);

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
        if (vertical != 0)
        {
            float moveSpeedToUse = vertical > 0 ? fowardMoveSpeed : backwardMoveSpeed;

            //Raycast and vector code here is 2nd attempt at fixing player floating bug (doesn't work but it's a start)
            RaycastHit rhInfo;
            Vector3 groundFwd = transform.forward;
            if(Physics.Raycast(transform.position, Vector3.down, out rhInfo))
            {
                groundFwd = Quaternion.AngleAxis(90.0f, transform.right) * rhInfo.normal;
            }

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
        if (Input.GetButtonDown("Dash") && (vertical != 0 || horizontalStrafe != 0))
        {
            playerEventsScript.PlayDashSound();
        }
        if (Input.GetButtonUp("Dash"))
        {
            playerEventsScript.StopDashSound();
        }

    }
}
