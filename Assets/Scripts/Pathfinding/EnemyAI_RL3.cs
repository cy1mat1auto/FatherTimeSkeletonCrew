using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_RL3 : MonoBehaviour
{
    //Rebuilding rigidbody AI, version 3
    public GameObject Ship, Player;
    public Rigidbody rb;
    public float Range = 250f, Close1 = 25f, Close2 = 50f, Cone = 45f, SpottingDelay = 0f;
    private float Highroad, Lowroad;
    private Vector3 Top, Bottom, Left, Right, ImmediateGoal;
    public bool Spotted, Avoiding, Searching;
    private RaycastHit view, front, lasthit;

    // Start is called before the first frame update
    void Start()
    {
        if (Ship == null)
        {
            Ship = gameObject;
        }

        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) >= Range)
        {
            //The Patrol() behavior can be overriden. The default behavior is a circular route
            Spotted = false;
            CancelInvoke("Seeking");
            Patrol();
        }

        else
        {
            //As long as the player is in Range, the ship will keep checking for FOV.
            InvokeRepeating("Seeking", 0f, 0.1f);
            if (!Spotted)
            {
                Patrol();
            }

            //To detect the player, line of sight within cone of vision must be unobstructed:
            if (view.transform.gameObject == Player && Vector3.Angle(transform.forward, Player.transform.position - transform.position) <= Cone)
            {
                Spotted = true;
                Debug.Log(Spotted);
            }
        }

        if (Spotted)
        {
            //This determines if there is line of sight to the player:
            if (view.collider.CompareTag("Player"))
            {
                Avoiding = false;
            }

            /*if (front.collider && !front.collider.CompareTag("Player"))
            {
                Avoiding = true;
            }*/

            //This determines what kind of object is obstructing line of sight:
            else if (view.collider.CompareTag("Enemy"))
            {
                Avoiding = true;
            }

            else if (view.collider.CompareTag("Terrain"))
            {
                Avoiding = true;
            }

            //This behavior is the ship's behavior when a straight path to the player is available:
            if (!Avoiding)
            {
                //At the absolute closest distance, the enemy ship cuts engines:
                if (Vector3.Distance(transform.position, Player.transform.position) >= Close1)
                {
                    rb.AddRelativeForce(new Vector3(0, 0, 120f), ForceMode.Force);
                }

                if (Vector3.Distance(transform.position, Player.transform.position) >= Close2)
                {
                    rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position, transform.up), Time.deltaTime * 4f);
                }

                //Withing the Close2 distance, the enemy ship matches orientation of the Player and continues to face the player:
                else
                {
                    rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position, Player.transform.up), Time.deltaTime * 2f);
                }
            }

            //The Avoiding behavior starts here. Note, because the enemy should not forget about the player's position during
            //obstacle-avoidance, "Spotted" is not deactivated by angle alone. This also gives you a chance to escape the area.
            else
            {

            }
            
        }
    }

    void Seeking()
    {
        Physics.Raycast(transform.position, Player.transform.position - transform.position, out view, Range);
        Physics.Raycast(transform.position, transform.forward, out front, Range);
    }

    public virtual void Patrol()
    {
        rb.AddRelativeForce(new Vector3(0, 0, 150f), ForceMode.Force);
        rb.rotation *= Quaternion.Euler(new Vector3(0, 0.4f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
