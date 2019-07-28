using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIDemo : MonoBehaviour
{
    //Decision-tree enemy AI by Richard
    //ShipRB will be used to move and rotate the enemy:
    public Rigidbody ShipRB;
    private RaycastHit view;

    //Alerted branches the enemy's behavior based on player's presence:
    private bool Alerted = false;

    //Prey is the player or player's ally, used for chasing and attacking;
    private GameObject Prey = null;

    //For fake path smoothing:
    private Vector3 TurnSmoother = new Vector3(0, 0, 0);

    //For delay in re-engaging after fly-by, and re-engaging at close range:
    public float delay = 5f;
    private float counter = 0;
    private Transform LastLocation;
    private bool WasAlerted = false;

    // Start is called before the first frame update
    void Start()
    {
        ShipRB = GetComponent<Rigidbody>();
        Prey = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<EnemyHealth>().CurrentHealth == 0)
        {
            Destroy(GetComponent<EnemyHBar2>().OverallBar);
            Destroy(gameObject);
        }

        if(Vector3.Distance(transform.position, Prey.transform.position) < 800f)
        {
            Alerted = true;
        }

        if (!Alerted)
        {
            //this should contain the enemy's patrolling behavior
            ShipRB.rotation *= Quaternion.Euler(new Vector3(0, 0.2f, 0));
        }

        else
        {
            //rotate the enemy using transform.LookAt:
            if (counter == 0)
            {
                if (Vector3.Distance(transform.position, Prey.transform.position) <= 500f && Vector3.Distance(transform.position, Prey.transform.position) >= 20f)
                {
                    transform.LookAt(Prey.transform.position);
                    ShipRB.AddRelativeForce(new Vector3(0, 0, 30000f), ForceMode.Force);
                }
            }

            else if (counter <= delay)
            {
                counter += Time.deltaTime;
                ShipRB.AddRelativeForce(new Vector3(0, 0, 10000f), ForceMode.Force);
            }

            else
            {
                if (Mathf.Abs(Vector3.Angle(transform.forward, (LastLocation.position - transform.position))) >= 5f)
                {
                    ShipRB.AddRelativeForce(new Vector3(0, 0, 7000f), ForceMode.Force);
                    ShipRB.rotation *= Quaternion.Euler(new Vector3(0, 0.6f, 0));
                }

                else
                {
                    transform.LookAt(LastLocation.position);
                    counter = 0;
                }

            }

        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().CurrentHealth -= 20f;
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(40000f, transform.position, 50f);
        }
    }
}
