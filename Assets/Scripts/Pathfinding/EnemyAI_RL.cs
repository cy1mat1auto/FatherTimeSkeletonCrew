using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_RL : MonoBehaviour
{
    //Decision-tree enemy AI by Richard
    //ShipRB will be used to move and rotate the enemy:
    public Rigidbody ShipRB;
    private RaycastHit view;

    //Alerted branches the enemy's behavior based on player's presence:
    private bool Alerted = false;

    //Prey is the player or player's ally, used for chasing and attacking;
    private GameObject Prey = null;

    // Start is called before the first frame update
    void Start()
    {
        ShipRB = GetComponent<Rigidbody>();
        Prey = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, (Prey.transform.position - transform.position), Color.red, 0.1f);
        if ((Mathf.Abs(Vector3.Angle(transform.forward, (Prey.transform.position - transform.position))) < 50f) && Vector3.Distance(transform.position, Prey.transform.position) < 500f)
        {
            if (Physics.Raycast(transform.position, (Prey.transform.position - transform.position), out view, 500f))
            {
                if (view.transform.tag == "Player")
                {
                    Alerted = true;
                    Debug.Log("spotted");
                }

                else
                {
                    Alerted = false;
                }

            }
        }

        if (!Alerted)
        {
            //this should contain the enemy's patrolling behavior
            ShipRB.AddRelativeForce(new Vector3(0, 0, 10f), ForceMode.Force);
            ShipRB.rotation *= Quaternion.Euler(new Vector3(0, 0.2f, 0));
        }
        
        else
        {
            //rotate the enemy by setting rigidboy rotation:
            //Quaternion rotation = Quaternion.LookRotation(Prey.transform.position - transform.position, transform.up);
            //ShipRB.rotation = rotation;

            //rotate the enemy using transform.LookAt:
            transform.LookAt(Prey.transform.position);

            if (Vector3.Distance(transform.position, Prey.transform.position) <= 500f && Vector3.Distance(transform.position, Prey.transform.position) >= 20f)
            {
                ShipRB.AddRelativeForce(new Vector3(0, 0, 15f), ForceMode.Force);
            }

        }
    }
}
