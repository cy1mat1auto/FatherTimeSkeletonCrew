using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    private int timer, startTime;
    private bool fired, homing;

    public Rigidbody rb;
    public Transform startObj;
    private Transform target;

    private float speed, acceleration, turnSpeed, turnAcceleration;
    public float maxSpeed, maxTurnSpeed;
    Quaternion lookRotation;

    public int order;
    private int orderTime;
    private int increment;

    public GameObject jockey01;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        startTime = 300;
        timer = startTime;
        fired = false;

        target = JockeyWeapons.missileTarget;
        acceleration = 0.05f;
        turnAcceleration = 0.002f;

        rb = GetComponent<Rigidbody>();

        orderTime = 0;
        increment = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && !fired)
        {
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
            orderTime++;
            if (order == 0 || orderTime / order >= increment)
            {
                if (timer == startTime)
                {
                    speed = jockey01.GetComponent<PlayerMove>().Speed;
                    resetMissile();
                }
                timer--;
                transform.GetComponentInChildren<Renderer>().enabled = true;
                rb.MovePosition(transform.position + transform.forward * speed);
                if (speed < maxSpeed)
                    speed += acceleration;
                if (homing)
                {
                    lookRotation = Quaternion.LookRotation((target.position - transform.position).normalized);
                    rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed));
                    if (turnSpeed < maxTurnSpeed)
                        turnSpeed += turnAcceleration;
                }
            }
        }
        else
        {
            speed = 0;
            turnSpeed = 0;
            homing = false;
            transform.GetComponentInChildren<Renderer>().enabled = false;
            orderTime = 0;
        }
    }

    void resetMissile()
    {
        transform.position = startObj.position;
        transform.rotation = startObj.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        Physics.IgnoreCollision(transform.GetComponent<Collider>(), jockey01.GetComponent<Collider>());
        if (collision.collider.tag == "Missile")
            Physics.IgnoreCollision(transform.GetComponent<Collider>(), collision.collider);

        if (fired)
        {
            if (collision.collider.tag == "Enemy")
                collision.gameObject.GetComponent<EnemyHealth>().CurrentHealth -= damage;

            fired = false;
            resetMissile();
            timer = startTime;
        }
    }
}
