using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript2 : MonoBehaviour
{
    private int timer, startTime;
    private int cooldown;
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
    public ParticleSystem explosion;
    public ParticleSystem smokeTrail;

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

        explosion.GetComponent<Renderer>().enabled = false;
        smokeTrail.Stop();
    }

    void Awake()
    {
        jockey01 = GameObject.FindGameObjectWithTag("Player");
        startObj = jockey01.transform.Find("PortLaser");
        fired = true;
        transform.position = startObj.position;
        transform.rotation = startObj.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && !fired && !explosion.isPlaying)
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
            explodeMissile();
            timer = startTime;
        }

        if (fired)
        {
            orderTime++;
            if (order == 0 || orderTime / order >= increment)
            {

                smokeTrail.transform.position = transform.position;
                smokeTrail.transform.rotation = transform.rotation;
                smokeTrail.Play();

                if (timer == startTime)
                {
                    speed = jockey01.GetComponent<PlayerMove>().Speed;
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

    void explodeMissile()
    {
        explosion.GetComponent<Renderer>().enabled = true;
        explosion.transform.position = transform.position;
        explosion.Play();
        smokeTrail.Stop();

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

            explodeMissile();

            fired = false;
            timer = startTime;
        }
    }
}
