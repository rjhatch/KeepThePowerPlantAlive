using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed;
    public float sprintModifier;
    public float gravity = -9.81f;
    public float jumpHeight;

    public bool isGrounded;
    public float groundCheckDistance = .2f;
    public Transform groundCheck;
    public LayerMask playerLayer;

    private Vector3 velocity;
    private float movementSpeed;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, ~playerLayer);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (isGrounded && Input.GetKey(KeyCode.LeftShift))
            movementSpeed = speed * sprintModifier;
        else
            movementSpeed = speed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * movementSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}
