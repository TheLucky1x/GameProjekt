using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

  public float mouseSensitivity = 100f;

  public Transform playerBody;

  float xRotation = 0f;

  // Start is called before the first frame update
  void Start()
  {
    // Lock the cursor in the screen to not accidently click somewhere outside of the game
    Cursor.lockState = CursorLockMode.Locked;
  }

  // Update is called once per frame
  void Update()
  {
      // Get the different inputs to look around
      float mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
      float mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;

      // Rotating
      xRotation -= mouseY; // Rotating the whole character (left/right)
      xRotation = Mathf.Clamp /* clamp so we're not able to look behind */(xRotation, -90f, 90f); // Only rotating the camera (up/down)

      transform.localRotation = Quaternion.Euler(xRotation,0f,0f);
      playerBody.Rotate(Vector3.up * mouseX);
  }
}
