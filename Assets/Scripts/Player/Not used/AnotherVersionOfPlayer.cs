﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherVersionOfPlayer : MonoBehaviour
{

    public float moveSpeed = 7;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8f;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;

    new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
      rigidbody = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {
      Vector3 inputDirection = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;
      float inputMagnitude = inputDirection.magnitude;
      smoothInputMagnitude = Mathf.SmoothDamp (smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

      float targetAngle = Mathf.Atan2 (inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
      angle = Mathf.LerpAngle (angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

      velocity = transform.forward * moveSpeed * smoothInputMagnitude;
    }

    void FixedUpdate() {
      rigidbody.MoveRotation (Quaternion.Euler (Vector3.up * angle));
      rigidbody.MovePosition (rigidbody.position + velocity * Time.deltaTime);
    }

}
