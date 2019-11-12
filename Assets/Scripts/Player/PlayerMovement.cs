using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller; // The Player itself

    public float speed = 12f; // Movement speed
    public float gravity = -9.81f; // How strong we are being pulled back to the ground
    public float jumpHeight = 3f; // How high we can Jump

    public Transform groundCheck; // Our groundCheck object
    public float groundDistance = 0.4f; // The distance the "groundCheck" checks for a "ground"
    public LayerMask groundMask; // To define for what to check

    Vector3 velocity; // Our move speed in the end
    bool isGrounded; // To check if we are on the ground or not

    // Update is called once per frame
    void Update()
    {
        // Checking if Player is on the ground or not
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask); // Creating a sphere around our object with the informations from above

        if(isGrounded && velocity.y < 0) /* if we are falling (velocity.y < 0) and are grounded (so we're touching the ground) set our velocity on the y axes to -2f (to be absolutely sure, i dont exactly know why) */{
          velocity.y = -2f;
        }

        // calculating the Movement depending on the input and where we look
        float x = Input.GetAxis("Horizontal"); // Our Horizontal movement input
        float z = Input.GetAxis("Vertical"); // Our Vertical movement input

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        // Jumping and gravity calculations
        if(Input.GetButtonDown("Jump") && isGrounded) /* If we're on the ground and press the jump button (standard: space), jump */ {
          velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime; // Move on the y axes (up/down (jump))

        controller.Move(velocity * Time.deltaTime); // Move on the x and z axes (left/right)
    }
}
