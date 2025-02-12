using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOuvement : MonoBehaviour

{
    public float moveSpeed = 5f; // Player's movement speed
    public float rotationSpeed = 700f; // Speed for both player and camera rotation
    public float cameraRotationSpeed = 2f; // Camera's vertical rotation speed
    public float gravity = -9.81f; // Gravity force
    public Transform groundCheck; // Reference to check if the player is grounded
    public LayerMask groundMask; // Layers for ground detection

    private CharacterController characterController;
    private Vector3 velocity; // The current velocity of the player
    private bool isGrounded; // To check if player is grounded
    private Camera playerCamera; // Reference to the player's camera

    private float xRotation = 0f; // To store the vertical camera rotation (pitch)

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main; // Assuming the camera is the main camera

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keep player grounded when they hit the ground
        }

        // Get movement input
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
        float moveZ = Input.GetAxis("Vertical"); // W/S or Up/Down Arrow keys

        // Calculate movement direction based on camera rotation
        Vector3 moveDirection = playerCamera.transform.forward * moveZ + playerCamera.transform.right * moveX;

        // Flatten move direction on the XZ plane (no vertical movement)
        moveDirection.y = 0;

        // Apply movement to the character controller
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Handle gravity (falling/jumping)
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraRotationSpeed * Time.deltaTime;

        // Rotate the player (horizontal rotation)
        transform.Rotate(0f, mouseX, 0f);

        // Rotate the camera (vertical rotation) and clamp it
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent over-rotation

        // Apply vertical camera rotation
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // Unlock cursor if desired (e.g., when pressing Escape or similar)
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}