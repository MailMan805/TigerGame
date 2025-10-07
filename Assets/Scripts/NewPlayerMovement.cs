using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerMovement : MonoBehaviour
{
    [Header("Player Movement Variables")]
    public float movementSpeed;
    public float jumpForce = 300f;
    public float mouseSensitivity = 50f;
    private float gravity = -9.81f;
    private float verticalRotation;
    private float verticalVelocity;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 cameraVelocity = Vector2.zero;
    private InputAction move;
    private InputAction look;
    private InputAction run;
    private InputAction crouch;
    private InputAction jump;
    private InputAction interact;

    [Header("Player Parts")]
    public Transform playerCamera;
    private TigerAI tiger;
    CapsuleCollider playerCollider;

    [Header("Player Movement Status")]
    private bool isMoving = false;
    private bool isGrounded = true;
    private bool isRunning = false;
    private bool isCrouching = false;

    public GameManager gameManager;

    public PlayerInputActions playerControls;

    private Rigidbody rb;

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        look = playerControls.Player.Look;
        look.Enable();

        run = playerControls.Player.Run;
        run.Enable();
        run.performed += StartRun;
        run.canceled += StopRun;

        crouch = playerControls.Player.Crouch;
        crouch.Enable();
        crouch.performed += StartCrouch;
        crouch.canceled += StopCrouch;

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;

        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        crouch.Disable();
        jump.Disable();
        interact.Disable();
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCollider = GetComponent<CapsuleCollider>();
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
        // Camera Look Around
        cameraVelocity = look.ReadValue<Vector2>();

        // Movement
        moveDirection = move.ReadValue<Vector2>();

        if (moveDirection != Vector2.zero)
        {
            isMoving = true;
        }
        else { isMoving  = false; }

        if (isRunning)
        {
            movementSpeed = 7f;
        }

        if (isCrouching)
        {
            movementSpeed = 2f;
        }

        #region player camera

        // Mouse input
        float mouseX = cameraVelocity.x * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = cameraVelocity.y * mouseSensitivity * Time.fixedDeltaTime;

        // Rotate player horizontally
        Quaternion deltaRotation = Quaternion.Euler(0f, mouseX, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);

        // Vertical rotation for the camera
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * moveDirection.y + right * moveDirection.x;
        desiredMoveDirection.Normalize();

        // Move the player
        Vector3 movement = desiredMoveDirection * movementSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        #endregion

        // Jumping
        if (verticalVelocity > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, verticalVelocity, rb.velocity.z);
            verticalVelocity = 0f;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!isCrouching)
        {
            print("Jump");
            verticalVelocity = Mathf.Sqrt(jumpForce * -1.2f * gravity);
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        print("Interact");
    }

    private void StartCrouch(InputAction.CallbackContext context)
    {
        print("Start Crouch");
        isCrouching = true;
        movementSpeed = 2f;
        playerCamera.localPosition = new Vector3(0f, 0.35f, 0f);
    }

    private void StopCrouch(InputAction.CallbackContext context)
    {
        print("Stop Crouch");
        isCrouching = false;
        movementSpeed = 5f;
        playerCamera.localPosition = new Vector3(0f, 0.5f, 0f);
    }

    private void StartRun(InputAction.CallbackContext context)
    {
        print("Start Run");
        isRunning = true;
    }

    private void StopRun(InputAction.CallbackContext context)
    {
        print("Stop Run");
        isRunning = false;
        movementSpeed = 5f;
    }

    
    public void AwarenessStates()
    {
        // Check for different movement and crouching states to adjust tiger's awareness
        if (isCrouching)
        {
            // Player is crouching, check if moving or not
            if (!isMoving)
            {
                tiger.awareness = 0.0f; // Crouching and not moving, low awareness
            }
            else
            {
                tiger.awareness = 1.0f; // Crouching and moving, moderate awareness
            }
        }
        else
        {
            // Player is not crouching, check movement speed
            if (isRunning)
            {
                tiger.awareness = 3.0f; // Player is running, high awareness
            }
            else if (isMoving)
            {
                tiger.awareness = 2.0f; // Player is walking, moderate-high awareness
            }
            else
            {
                tiger.awareness = 1.0f; // Player is standing still
            }
        }


    }

}
