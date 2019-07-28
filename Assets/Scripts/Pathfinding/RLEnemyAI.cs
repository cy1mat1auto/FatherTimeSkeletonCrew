using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLEnemyAI : MovementBehaviour
{
    //Decision-tree enemy AI by Richard. Adapted for as MovementBehaviour class
    //ShipRB will be used to move and rotate the enemy:
    public Rigidbody ShipRB;
    private RaycastHit view;

    //Prey is the player or player's ally, used for chasing and attacking;
    private GameObject Prey = null;

    //For fake path smoothing:
    private Vector3 TurnSmoother = new Vector3(0, 0, 0);

    //For delay in re-engaging after fly-by, and re-engaging at close range:
    public float delay = 5f;
    private float counter = 0;
    private Transform LastLocation;
    private bool WasAlerted = false;

    private void Awake()
    {
        priority = 0.5f;
        ShipRB = GetComponent<Rigidbody>();
        detectionRadius = 50000f;
    }

    public override void SetTarget(GameObject target)
    {
        Prey = target;
    }

    public void Update()
    {
        //Debug.DrawRay(transform.position, (Prey.transform.position - transform.position), Color.red, 0.1f);
        //This part determines if the enemy has spotted the player; the first branch point of the decision tree:
        if ((!activated) && (Prey != null))
        {
            if ((Mathf.Abs(Vector3.Angle(transform.forward, (Prey.transform.position - transform.position))) < 50f) && Vector3.Distance(transform.position, Prey.transform.position) < detectionRadius)
            {
                if (Physics.Raycast(transform.position, (Prey.transform.position - transform.position), out view, detectionRadius))
                    activated = view.transform.tag == "Player" ? true : false;
                if (activated)
                    PingParent();
            }
            else
                print((Vector3.Distance(transform.position, Prey.transform.position) < detectionRadius));
        }

    }

    public override bool ExecuteBehaviour()
    {
        /*
        if (GetComponent<EnemyHealth>().CurrentHealth == 0)
        {
            Destroy(GetComponent<EnemyHBar2>().OverallBar);
            Destroy(gameObject);
        }

        if (!Alerted)
        {
            //this should contain the enemy's patrolling behavior
            ShipRB.AddRelativeForce(new Vector3(0, 0, 10f), ForceMode.Force);
            ShipRB.rotation *= Quaternion.Euler(new Vector3(0, 0.2f, 0));
        }
        */

        //else
        // {
        //rotate the enemy by setting rigidboy rotation:
        //Quaternion rotation = Quaternion.LookRotation(Prey.transform.position - transform.position, transform.up);
        //ShipRB.rotation = rotation;

        //rotate the enemy using transform.LookAt:
        if (Prey == null)
        {
            activated = false;
            return activated;
        }
        if (counter == 0)
        {
            if (Vector3.Distance(transform.position, Prey.transform.position) <= detectionRadius && Vector3.Distance(transform.position, Prey.transform.position) >= 60f)
            {
                transform.LookAt(Prey.transform.position);
                ShipRB.AddRelativeForce(new Vector3(0, 0, 150f), ForceMode.Force);
            }
            else if (Vector3.Distance(transform.position, Prey.transform.position) < 60f)
            {
                //Debug.DrawRay(transform.position, (Prey.transform.TransformPoint(Prey.transform.localPosition + new Vector3(0, 20f, 0)) - transform.position), Color.red, 0.1f);
                if (Mathf.Abs(Vector3.Angle(transform.forward, (Prey.transform.position + new Vector3(0, 20f, 0) - transform.position))) >= 2f)
                {
                    TurnSmoother += new Vector3(0f, 0.5f, 0f);
                    transform.LookAt(Prey.transform.position + TurnSmoother);
                    ShipRB.AddRelativeForce(new Vector3(0, 0, 15f), ForceMode.Force);
                }

                else
                {
                    ShipRB.AddRelativeForce(new Vector3(0, 0, 15f), ForceMode.Force);
                    LastLocation = Prey.transform;
                    TurnSmoother = new Vector3(0, 0, 0);
                    counter += Time.deltaTime;
                    Debug.Log(TurnSmoother);
                }
            }
            else
                activated = false;
        }
        else if (counter <= delay)
        {
            counter += Time.deltaTime;
            ShipRB.AddRelativeForce(new Vector3(0, 0, 30f), ForceMode.Force);
        }
        else
        {
            if (Mathf.Abs(Vector3.Angle(transform.forward, (LastLocation.position - transform.position))) >= 5f)
            {
                ShipRB.AddRelativeForce(new Vector3(0, 0, 15f), ForceMode.Force);
                ShipRB.rotation *= Quaternion.Euler(new Vector3(0, 0.6f, 0));
            }
            else
            {
                transform.LookAt(LastLocation.position);
                counter = 0;
            }
        }
        return activated;
    //    }
    }

    public override void EndBehaviour()
    {
        activated = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;   // Maybe do something else if this is default behaviour
    }
}
