using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
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
    void Awake()
    {
       
        fired = true;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        acceleration = 0.05f;
        turnAcceleration = 0.002f;

        rb = GetComponent<Rigidbody>();

        orderTime = 0;
        increment = 20;

        explosion.GetComponent<Renderer>().enabled = false;
        smokeTrail.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }
}
