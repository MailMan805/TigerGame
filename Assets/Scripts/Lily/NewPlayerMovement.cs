using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewPlayerMovement : MonoBehaviour
{
    public static NewPlayerMovement Instance;
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
    // private InputAction interact;
    private InputAction map;

    [Header("UI Stuff")]
    public RawImage crouchingUI;
    public RawImage standingUI;
    public RawImage grabbingUI;
    public RawImage lookingUI;
    public RawImage mapUI;
    private Vector2 mapStartPosition = new Vector2(0f, -700f);
    private Vector2 mapEndPosition = new Vector2(0f, 0f);
    private float mapLerpDuration = 1.0f;
    private float mapLerpSpeed = 8f;

    public RawImage SpawnMarker;
    public RawImage DeadForestMarker;
    public RawImage TowerMarker;
    public RawImage StatueMarker;
    public RawImage RockMarker;
    public RawImage WellMarker;
    public RawImage BushesMarker;

    public bool bodyLook = false;

    [Header("Player Parts")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Transform playerCamera;
    private TigerAI tiger;
    CapsuleCollider playerCollider;

    [Header("Player Movement Status")]
    public bool canMove = true;
    private bool isMoving = false;
    private bool isGrounded = true;
    private bool isRunning = false;
    public bool isCrouching = false;
    private bool isLookingAtMap = false;

    [Header("Game Manager")]
    public GameManager gameManager;

    public PlayerInputActions playerControls;

    private Rigidbody rb;

    public GameObject playerTouchingBody;

    public void OnEnable()
    {


        map = playerControls.Player.Map;
        map.Enable();
        map.performed += StartMap;
        map.canceled += StopMap;

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
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        crouch.Disable();
        jump.Disable();
        map.Disable();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        playerControls = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        tiger = FindAnyObjectByType<TigerAI>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        gameManager = GameManager.instance;

        mapUI.rectTransform.anchoredPosition = mapStartPosition;
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
        else { isMoving = false; }

        if (isRunning && canMove)
        {
            movementSpeed = 7f;
        }

        #region ui handler

        if (isCrouching && !bodyLook && canMove)
        {
            movementSpeed = 2f;
            standingUI.enabled = false;
            crouchingUI.enabled = true;
        }
        else if(!bodyLook)
        {
            standingUI.enabled = true;
            crouchingUI.enabled = false;
        }

        // Player looking at tiger UI
        if (tiger != null && tiger.isPlayerLooking && tiger.playerDistance < 50f && !bodyLook)
        {
            lookingUI.enabled = true;
            standingUI.enabled = false;
            crouchingUI.enabled = false;
        }
        else if (!bodyLook)
        {
            lookingUI.enabled = false;
        }

        
        #endregion

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
        // Ground Check
        RaycastHit hit;
        float groundCheckDistance = 1.1f;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
            verticalVelocity = 0f;
        }
        else
        {
            isGrounded = false;
        }

        // Map UI
        if (isLookingAtMap)
        {
            mapLerpDuration -= Time.deltaTime * mapLerpSpeed;
        }
        else
        {
            mapLerpDuration += Time.deltaTime * mapLerpSpeed;
        }

        mapLerpDuration = Mathf.Clamp01(mapLerpDuration);

        mapUI.rectTransform.anchoredPosition = Vector2.Lerp(mapEndPosition, mapStartPosition, mapLerpDuration);
    }

    #region player input action
    private void Jump(InputAction.CallbackContext context)
    {
        if (!isCrouching && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpForce * -1.2f * gravity);
        }
    }

    //private void Interact(InputAction.CallbackContext context)
    //{
    //    print("Interact");
    //    body.CollectBody();
    //}

    private void StartCrouch(InputAction.CallbackContext context)
    {
        isCrouching = true;
        movementSpeed = 2f;
        playerCamera.localPosition = new Vector3(0f, 0.35f, 0f);
    }

    private void StopCrouch(InputAction.CallbackContext context)
    {
        isCrouching = false;
        movementSpeed = 5f;
        playerCamera.localPosition = new Vector3(0f, 0.5f, 0f);
    }

    private void StartRun(InputAction.CallbackContext context)
    {
        isRunning = true;
    }

    private void StopRun(InputAction.CallbackContext context)
    {
        isRunning = false;
        movementSpeed = 5f;
    }

    private void StartMap(InputAction.CallbackContext context)
    {
        isLookingAtMap = true;
    }

    private void StopMap(InputAction.CallbackContext context)
    {
        isLookingAtMap = false;
    }

    #endregion

    public void AwarenessStates()
    {
        if(tiger != null)
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

}
