using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraV4 : MonoBehaviour
{
    //A 3D platformer-type camera controller, from the Father Time project before I broke that test environment.
    public float turnSpeed = 1.0f;
    public float height = 8f;
    public float distance = 10f;
    public Transform player;

    private Vector3 offset;
    private Quaternion updownX;
    private Quaternion updownZ;
    private Quaternion leftright;

    void Start()
    {
        //Change player to the appropriate player gameobject
        //player = GameObject.Find("Bucky/CameraTarget").transform;
        offset = new Vector3(0, height, distance);
        updownX = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.right);
        updownZ = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.forward);
        leftright = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up);
    }

    void LateUpdate()
    {
        leftright = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up);

        if (transform.eulerAngles.y <= 270 && transform.eulerAngles.y >= 90)
        {
            updownX = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.right);
            //updownZ = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.forward);
        }

        else
        {
            updownX = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * turnSpeed, Vector3.right);
            //updownZ = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.forward);
        }

        if (transform.eulerAngles.y == 0  || transform.eulerAngles.y >= 180)
        {
            //updownX = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.right);
            updownZ = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * turnSpeed, Vector3.forward);
        }

        else
        {
            //updownX = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * turnSpeed, Vector3.right);
            updownZ = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.forward);
        }

        offset = updownX * updownZ * leftright * offset;
        transform.position = player.position + offset*3;
        transform.LookAt(player.position);
       
    }
}
