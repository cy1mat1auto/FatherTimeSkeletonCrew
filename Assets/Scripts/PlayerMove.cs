﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed;
    public float Sensitivity;
    public bool canMove = true;
    public bool Inverted = true;
    private int Inv = 1;


    //Thrusters
    private PlayerThruster thruster;
    private float thrusterTimer = 0f;
    private readonly float maxThrusterTimer = 2f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        thruster = GetComponentInChildren<PlayerThruster>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Inverted)
        {
            Inv = 1;
        }

        else
        {
            Inv = -1;
        }

        if (canMove)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * Sensitivity, Vector3.forward);
            }

            else
            {
                transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * Sensitivity, Inv * Vector3.right);
                transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * Sensitivity, Vector3.up);
            }

            if (Input.GetKey(KeyCode.V))
            {
                GetComponent<Rigidbody>().drag = 6f;
            }

            else
            {
                GetComponent<Rigidbody>().drag = 1;
            }


            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                GetComponent<Rigidbody>().velocity += Speed * 5f * transform.forward;

                thrusterTimer += 0.5f * Time.deltaTime;
                if (thrusterTimer >= maxThrusterTimer)
                    thrusterTimer = maxThrusterTimer;
            }

            else if (Input.GetKey(KeyCode.W))
            {
                GetComponent<Rigidbody>().velocity += Speed * transform.forward;

                thrusterTimer -= 0.3f * Time.deltaTime;

                if (thrusterTimer <= 0.05f)
                    thrusterTimer = 0.05f;
            }
            else
            {
                thrusterTimer -= 0.8f * Time.deltaTime;
                if (thrusterTimer <= 0)
                    thrusterTimer = 0;
            }

            if (Input.GetKey(KeyCode.S))
            {
                GetComponent<Rigidbody>().velocity += 0.5f * Speed * -transform.forward;
            }

            if (Input.GetKey(KeyCode.A))
            {
                GetComponent<Rigidbody>().velocity += 0.7f * Speed * -transform.right;
            }

            if (Input.GetKey(KeyCode.D))
            {
                GetComponent<Rigidbody>().velocity += 0.7f * Speed * transform.right;
            }
        }
        thruster.SetNewEmission(thrusterTimer);
    }
}
