using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Variables")]
    public float movementSpeed;
    public float jumpForce = 300f;
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
    public RawImage crouchingIcon;
    public RawImage standingIcon;
    public RawImage grabIcon;


    [Header("Player Movement Status")]
    private bool isGrounded;
    private bool isRunning;
    private bool isCrouching;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCollider = GetComponent<CapsuleCollider>();
        characterController = GetComponent<CharacterController>();
        tiger = FindAnyObjectByType<TigerAI>();

        standingIcon.gameObject.SetActive(true);
        crouchingIcon.gameObject.SetActive(false);
        grabIcon.gameObject.SetActive(false);
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
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

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
        isCrouching = (isGrounded && Input.GetKey(KeyCode.LeftControl));

        float targetHeight = isCrouching ? 1.0f : 2.0f;
        Vector3 targetCamPos = isCrouching ? new Vector3(0f, 0.2f, 0f) : new Vector3(0f, 0.5f, 0f);
        float crouchLerpSpeed = 8f;

        playerCollider.height = Mathf.Lerp(playerCollider.height, targetHeight, Time.deltaTime * crouchLerpSpeed);
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, targetCamPos, Time.deltaTime * crouchLerpSpeed);

        if (isCrouching)
        {
            isCrouching = true;
            movementSpeed = 1.5f;
            standingIcon.gameObject.SetActive(false);
            crouchingIcon.gameObject.SetActive(true);
        }
        else
        {
            isCrouching = false;
            standingIcon.gameObject.SetActive(true);
            crouchingIcon.gameObject.SetActive(false);
        }

        // Movement
        moveNS = Input.GetAxis("Vertical");
        moveEW = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * moveEW + transform.forward * moveNS;

        if (isGrounded && Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            isRunning = true;
            movementSpeed = 5.0f;
        }
        else if (!isCrouching)
        {
            isRunning = false;
            movementSpeed = 3.0f;
        }

        move *= movementSpeed;

        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);
    }

    public void AwarenessStates()
    {
        if (isRunning && !isCrouching) { tiger.awareness = 3.0f; } // Player is running
        else if ((moveNS != 0) || (moveEW != 0) && !isCrouching) { tiger.awareness = 2.0f; } // Player is walking
        else if ((moveNS == 0) && (moveEW == 0) && isCrouching) { tiger.awareness = 0.0f; } // Player standing still with no movement at all
        else if ((moveNS == 0) && (moveEW == 0) || isCrouching) { tiger.awareness = 1.0f; } // Player standing still or crouching
    }
}
