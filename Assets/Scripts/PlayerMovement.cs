using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Variables")]
    public float movementSpeed;
    public float jumpForce = 0.5f;
    public float mouseSensitivity = 2.0f;
    private float gravity = -9.81f;
    private float verticalRotation;
    private float verticalVelocity;
    private float moveNS, moveEW;
    private Vector3 cameraVelocity = Vector3.zero;

    [Header("Player Parts")]
    public Transform playerCamera;
    private CharacterController characterController;
    private TigerAI tiger;
    CapsuleCollider playerCollider;

    [Header("Player Movement Status")]
    private bool isGrounded;
    private bool isRunning;
    private bool isCrouching;

    public GameManager gameManager;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCollider = GetComponent<CapsuleCollider>();
        characterController = GetComponent<CharacterController>();
        tiger = FindAnyObjectByType<TigerAI>();
        gameManager = GameManager.instance;
    }

    void Update()
    {
        PlayerInput();

        AwarenessStates();
    }

    public void PlayerInput()
    {
        // Looking Around
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        if(!gameManager.inItemMenu)
        {
            transform.Rotate(0, horizontalRotation, 0);
            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }

        // Jumping
        isGrounded = characterController.isGrounded;

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // keeps player grounded
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = Mathf.Sqrt(jumpForce * -1.2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        // Crouching
        if (isGrounded && Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            movementSpeed = 3f;
            playerCollider.height = 1.0f;
            playerCamera.localPosition = new Vector3(0f, 0.2f, 0f);
        }
        else
        {
            isCrouching = false;
            playerCollider.height = 2.0f;
            playerCamera.localPosition = new Vector3(0f, 0.5f, 0f);
        }

        // Movement
        moveNS = Input.GetAxis("Vertical");
        moveEW = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * moveEW + transform.forward * moveNS;

        if (isGrounded && Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            isRunning = true;
            movementSpeed = 7.0f;
        }
        else if (!isCrouching)
        {
            isRunning = false;
            movementSpeed = 5f;
        }

        move *= movementSpeed;

        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);
    }

    public void AwarenessStates()
    {
        if (isRunning && !isCrouching) { tiger.awareness = 3.0f; } // Player is running
        else if ((moveNS == 0) && (moveEW == 0) && isCrouching) { tiger.awareness = 0.0f; } // Player standing still with no movement at all
        else if (((moveNS == 0) && (moveEW == 0)) || (isCrouching && (moveNS != 0) || (moveEW != 0))) { tiger.awareness = 1.0f; } // Player standing still or crouching
        else if ((moveNS != 0) || (moveEW != 0) && !isCrouching) { tiger.awareness = 2.0f; } // Player is walking
    }
}
