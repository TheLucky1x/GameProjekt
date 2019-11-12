using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public event System.Action OnReachedEndOfLevel;

    public CharacterController controller; // The Player itself

    public float speed = 12f; // Movement speed
    public float gravity = -9.81f; // How strong we are being pulled back to the ground
    public float jumpHeight = 3f; // How high we can Jump

    public Transform groundCheck; // Our groundCheck object
    public float groundDistance = 0.4f; // The distance the "groundCheck" checks for a "ground"
    public LayerMask groundMask; // To define for what to check

    Vector3 velocity; // Our move speed in the end
    bool isGrounded; // To check if we are on the ground or not
    bool disabled; // To disable the controlls, for example if a guard detects us

    void Start() {
      Guard.OnGuardHasSpottedPlayer += Disable; // "Subscribe" our selfmade function called "Disable" to the function "OnGuardHasSpottedPlayer" inside the Guard script
    }

    // Update is called once per frame
    void Update()
    {
        // calculating the Movement depending on the input and where we look
        float x = Input.GetAxis("Horizontal"); // Our Horizontal movement input
        float z = Input.GetAxis("Vertical"); // Our Vertical movement input

        Vector3 move = Vector3.zero; // Setting "move" on zero
        if (!disabled) /* If we're NOT disabled, do the following */ {
          move = transform.right * x + transform.forward * z; // Only if we aren't disabled are we allowed to move
        }

        // Checking if Player is on the ground or not
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask); // Creating a sphere around our object with the informations from above

        if(isGrounded && velocity.y < 0) /* if we are falling (velocity.y < 0) and are grounded (so we're touching the ground) set our velocity on the y axes to -2f (to be absolutely sure, i dont exactly know why) */{
          velocity.y = -2f;
        }



        controller.Move(move * speed * Time.deltaTime);

        // Jumping and gravity calculations
        if(Input.GetButtonDown("Jump") && isGrounded) /* If we're on the ground and press the jump button (standard: space), jump */ {
          velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime; // Move on the y axes (up/down (jump))

        controller.Move(velocity * Time.deltaTime); // Move on the x and z axes (left/right)
    }

    void OnTriggerEnter(Collider hitCollider) {
      if (hitCollider.tag == "Finish") {
        Disable();
        if (OnReachedEndOfLevel != null) {
          OnReachedEndOfLevel();
        }
      }
    }

    void Disable() {
      disabled = true; // Set disabled to true so we can't move anymore
    }

    void OnDestroy() {
      Guard.OnGuardHasSpottedPlayer -= Disable; // To avoid getting errors we're unsubscribing our function because the object no longer exists
    }

}
