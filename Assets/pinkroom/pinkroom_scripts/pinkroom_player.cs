using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Déplacement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Caméra")]
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;
    public float lookLimit = 80f;

    private CharacterController controller;
    private float verticalSpeed = 0f;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        RotateCamera();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move *= moveSpeed;

        // Appliquer la gravité
        if (controller.isGrounded && verticalSpeed < 0)
            verticalSpeed = -2f;

        verticalSpeed += gravity * Time.deltaTime;
        move.y = verticalSpeed;

        controller.Move(move * Time.deltaTime);
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -lookLimit, lookLimit);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}

