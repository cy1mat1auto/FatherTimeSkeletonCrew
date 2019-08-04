using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    public float damage = 10f, starttime, timer = 5f;
    private Rigidbody rb;
    private Collider shell;
    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }
        shell = gameObject.GetComponent<Collider>();
    }

    private void Awake()
    {
        starttime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Time.time - starttime < timer)
        {
            rb.AddRelativeForce(new Vector3(0, 0, 80f), ForceMode.Acceleration);
        }

        else
        {
            Destroy(gameObject);
        }
        
    }

    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.tag == "Missile" || collision.collider.tag == "Enemy")
        {
            Physics.IgnoreCollision(transform.GetComponent<Collider>(), collision.collider);
        }

        else
        {
            if (collision.collider.tag == "Player")
            {
                if (collision.gameObject.GetComponent<PlayerHealth>())
                {
                    collision.gameObject.GetComponent<PlayerHealth>().CurrentHealth -= damage;
                }

                Destroy(gameObject);
            }
            
            
            //explodeMissile();

            
        }
    }
}
