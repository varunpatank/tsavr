using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 3f;
    private float gravity = -9.81f;
    private float velocityY = 0f;

    void Update()
    {
        // Move the XR Origin based on camera direction
        Transform cam = Camera.main.transform;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move relative to camera direction
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * z + right * x;

        // Apply gravity
        if (controller.isGrounded)
            velocityY = -1f;
        else
            velocityY += gravity * Time.deltaTime;

        move.y = velocityY;

        controller.Move(move * speed * Time.deltaTime);
    }
}