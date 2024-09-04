using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Pelaajan normaaliliikenopeus
    public float dashSpeed = 10f; // Dash-vauhti
    public float dashDuration = 0.2f; // Dashin kesto
    public float dashCooldown = 1f; // Dashin jäähdytinaika
    public float jumpHeight = 2f; // Hypyn korkeus
    public float gravity = -9.81f; // Painovoima

    public float lookSpeed = 2f; // Kameran kääntymisnopeus

    private CharacterController characterController;
    private Transform playerCamera;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private bool isDashing = false;
    private float dashEndTime = 0f;
    private float lastDashTime = 0f;

    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Pieni negatiivinen arvo pitää pelaajan maassa
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }

        if (isDashing && Time.time >= dashEndTime)
        {
            StopDash();
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        MovePlayer();
        LookAround();
        ApplyGravity();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        float currentSpeed = isDashing ? dashSpeed : moveSpeed;
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX * lookSpeed);

        verticalRotation -= mouseY * lookSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void StartDash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        lastDashTime = Time.time;
    }

    void StopDash()
    {
        isDashing = false;
    }
}
