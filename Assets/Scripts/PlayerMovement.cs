using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpForce = 300f;
    public float mouseSensitivity = 2.0f;
    public float gravity = -9.81f;

    public Transform playerCamera;

    private CharacterController characterController;
    private float verticalRotation;
    private float verticalVelocity;
    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
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
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        // Movement
        float moveNS = Input.GetAxis("Vertical");
        float moveEW = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * moveEW + transform.forward * moveNS;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = 6.0f;
        }
        else if (Input.GetKey(KeyCode.Quote))
        {
            print("Speed. I am speed. - Lightning McQueen");
            movementSpeed = 100.0f;
        }
        else
        {
            movementSpeed = 4.0f;
        }

        move *= movementSpeed;

        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);

    }
}
