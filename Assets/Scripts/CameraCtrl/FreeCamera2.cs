﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera2 : MonoBehaviour
{
    //Use this script to free-rotate the camera while the Jockey is stationary on a landign pad
    public bool FreeRotate;
    public GameObject PlayerView;

    //DefaultOffset is the angle of the camera relative to the shop's model when the ship is flying.
    public Vector3 DefaultOffset = new Vector3(4, 0, 0);
    private float Sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        Sensitivity = GetComponentInParent<PlayerMove>().Sensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if (FreeRotate)
        {
            PlayerView.transform.localRotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * Sensitivity, -Vector3.right);
            transform.localRotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * Sensitivity, Vector3.up);
        }

        else
        {
            transform.localEulerAngles = DefaultOffset;
            PlayerView.transform.localEulerAngles = new Vector3(0,0,0);
        }
    }
}
