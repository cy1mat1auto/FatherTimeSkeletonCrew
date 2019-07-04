using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript2 : MonoBehaviour
{
    private int timer, startTime;
    private int cooldown;
    private bool fired;
    public bool homing = false;

    public Rigidbody rb;
    public Transform startObj;
    public Transform target = null;

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
        Physics.IgnoreCollision(transform.GetComponent<Collider>(), jockey01.GetComponent<Collider>());
        startTime = 300;
        timer = startTime;
        fired = true;

        acceleration = 0.05f;
        turnAcceleration = 0.002f;

        rb = GetComponent<Rigidbody>();

        orderTime = 0;
        increment = 0;

        explosion.GetComponent<Renderer>().enabled = false;
        smokeTrail.Stop();
    }

    void Awake()
    {
        //jockey01 = GameObject.FindGameObjectWithTag("Player");
        //startObj = jockey01.transform.Find("PortLaser");
        fired = true;
        //transform.position = startObj.position;
        //transform.rotation = startObj.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!fired && !explosion.isPlaying)
        {
            fired = true;
            resetMissile();
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
            if (order == 0)// || orderTime / order >= increment)
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

    void resetMissile()
    {
        transform.position = startObj.position;
        transform.rotation = startObj.rotation;
    }

    void explodeMissile()
    {
        explosion.GetComponent<Renderer>().enabled = true;
        explosion.transform.position = transform.position;
        explosion.Play();
        smokeTrail.Stop();
        Invoke("RemoveMissile", 1.5f);
    }

    void RemoveMissile()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (fired && collision.collider.tag == "Missile")
        {
            Physics.IgnoreCollision(transform.GetComponent<Collider>(), collision.collider);
        }
 
        else if (fired)
        {
            if (collision.collider.tag == "Enemy")
                if (collision.gameObject.GetComponent<EnemyHealth>())
                {
                    collision.gameObject.GetComponent<EnemyHealth>().CurrentHealth -= damage;
                }

            explodeMissile();

            fired = false;
            timer = startTime;
        }
    }
}
