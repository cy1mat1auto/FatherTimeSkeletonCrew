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

        //Debug.DrawRay(transform.position, (Prey.transform.position - transform.position), Color.red, 0.1f);
        //This part determines if the enemy has spotted the player; the first branch point of the decision tree:
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
            if (counter == 0)
            {
                if (Vector3.Distance(transform.position, Prey.transform.position) <= 500f && Vector3.Distance(transform.position, Prey.transform.position) >= 60f)
                {
                    transform.LookAt(Prey.transform.position);
                    ShipRB.AddRelativeForce(new Vector3(0, 0, 30f), ForceMode.Force);
                }

                else if (Vector3.Distance(transform.position, Prey.transform.position) < 60f)
                {
                    //Debug.DrawRay(transform.position, (Prey.transform.TransformPoint(Prey.transform.localPosition + new Vector3(0, 20f, 0)) - transform.position), Color.red, 0.1f);
                    if (Mathf.Abs(Vector3.Angle(transform.forward, (Prey.transform.position + new Vector3(0, 30f, 0) - transform.position))) >= 2f)
                    {

                        TurnSmoother += new Vector3(0f, 0.5f, 0f);
                        transform.LookAt(Prey.transform.position + TurnSmoother);
                        ShipRB.AddRelativeForce(new Vector3(0, 0, 30f), ForceMode.Force);
                    }

                    else
                    {
                        ShipRB.AddRelativeForce(new Vector3(0, 0, 30f), ForceMode.Force);
                        LastLocation = Prey.transform;
                        TurnSmoother = new Vector3(0, 0, 0);
                        counter += Time.deltaTime;
                        Debug.Log(TurnSmoother);
                    }
                }

            }

            else if (counter <= delay)
            {
                counter += Time.deltaTime;
                ShipRB.AddRelativeForce(new Vector3(0, 0, 45f), ForceMode.Force);
            }

            else
            {
                if(Mathf.Abs(Vector3.Angle(transform.forward, (LastLocation.position - transform.position))) >= 5f)
                {
                    ShipRB.AddRelativeForce(new Vector3(0, 0, 50f), ForceMode.Force);
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
}
