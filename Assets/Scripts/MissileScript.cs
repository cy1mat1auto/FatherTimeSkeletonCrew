using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    private int timer, startTime;
    private bool fired, homing;
    public Transform startObj;
    private Transform target;
    private float speed, acceleration, turnSpeed, turnAcceleration;
    public float maxSpeed;
    Quaternion lookRotation;

    // Start is called before the first frame update
    void Start()
    {
        startTime = 300;
        timer = startTime;
        fired = false;
        target = JockeyWeapons.missileTarget;
        acceleration = 0.05f;
        turnAcceleration = 0.002f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && !fired)
        {
            resetMissile();
            fired = true;
            if (JockeyWeapons.onObject)
            {
                homing = true;
                target = JockeyWeapons.missileTarget;
            }
        }
        if (timer <= 0)
        {
            fired = false;
            resetMissile();
            timer = startTime;
        }

        if (fired)
        {
            timer--;
            transform.GetComponentInChildren<Renderer>().enabled = true;
            transform.Translate(transform.forward * speed, Space.World);
            if (speed < maxSpeed)
            {
                speed += acceleration;
            }
            if (homing)
            {
                lookRotation = Quaternion.LookRotation((target.position - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed);
                turnSpeed += turnAcceleration;
            }
        }
        else
        {
            speed = 0;
            turnSpeed = 0;
            homing = false;
            transform.GetComponentInChildren<Renderer>().enabled = false;
        }
    }

    void resetMissile()
    {
        transform.position = startObj.position;
        transform.rotation = startObj.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (fired)
        {
            fired = false;
            Debug.Log("hi");
        }
    }
}
