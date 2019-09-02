using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_RL3 : MonoBehaviour
{
    
    //Rebuilding rigidbody AI, version 3
    public GameObject Ship, Player;
    public Rigidbody rb;
    public float Range = 250f, Close1 = 25f, Close2 = 50f, Cone = 45f;
    public float SpottingDelay = 0f, AttackStart, AttackDuration = 3f, DisengageStart, DisengageDuration = 1f, SearchStart, SearchTime = 5f;
    private float Highroad, Lowroad, Leftroad, Rightroad;
    private Vector3 Top, Bottom, Left, Right, ImmediateGoal, LastPosition;
    public bool Spotted, Avoiding, Locked, Pursuit, Attacking, Disengaging, Searching;
    private bool Seek;
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
        if (Searching == true)
        {
            Pursuit = false;
            Attacking = false;
            Disengaging = false;
        }

        if (Disengaging == true)
        {
            Pursuit = false;
            Attacking = false;
            Searching = false;
        }

        if (Attacking == true)
        {
            Pursuit = false;
            Disengaging = false;
            Searching = false;
        }

        if (Pursuit == true)
        {
            Attacking = false;
            Disengaging = false;
            Searching = false;
        }
        
        if (Vector3.Distance(transform.position, Player.transform.position) >= Range)
        {
            //The Patrol() behavior can be overriden. The default behavior is a circular route
            Spotted = false;
            CancelInvoke("Seeking");
            Seek = false;
            Patrol();
        }

        //As long as the player is in Range, the ship will keep checking for FOV.
        else
        {
            if (Seek == false)
            {
                InvokeRepeating("Seeking", 0f, 0.1f);
                Seek = true;
            }

            if (!Spotted)
            {
                if (!Searching)
                {
                    Patrol();
                }
            }

            //To detect the player, line of sight within cone of vision must be unobstructed:
            if (view.transform.gameObject == Player && Vector3.Angle(transform.forward, Player.transform.position - transform.position) <= Cone)
            {
                Spotted = true;
                //Debug.Log(Spotted);
            }
        }

        if (Spotted)
        {
            //This determines if there is line of sight to the player, assuming it's in the "Pursuit" phase:
            if (view.collider.CompareTag("Player"))
            {
                Avoiding = false;
                //If there is line of sight and player is within FOV, update the player's most recent known position:
                if (Vector3.Angle(transform.forward, Player.transform.position - transform.position) <= Cone)
                {
                    LastPosition = Player.transform.position;
                    Locked = false;
                }
            }

            /*if (front.collider && !front.collider.CompareTag("Player"))
            {
                Avoiding = true;
            }*/

            //This determines what kind of object is obstructing line of sight:
            else if (view.collider.CompareTag("Enemy"))
            {
                //Avoiding = true;
            }

            else if (view.collider.CompareTag("Terrain"))
            {
                Avoiding = true;
            }

            else if (view.collider.CompareTag("LargeTerrain"))
            {
                
            }

            //This behavior is the ship's behavior when a straight path to the player is available:
            if (!Avoiding)
            {
                //The enemy ship's "engine" will keep it moving forward if it's not too close to the player:
                if (Vector3.Distance(transform.position, Player.transform.position) >= Close1)
                {
                    rb.AddRelativeForce(new Vector3(0, 0, 120f), ForceMode.Force);
                }

                //If the enemy ship gets close to the player, initiate an attack pattern, and disengage:

                //The attack pattern, Disengaging and Search toggle between one another in the "else statement"
                //They phases are written in reverse-chronological order, since later phase take precedence over earlier phase:
                else
                {
                    
                    if (Disengaging)
                    {
                        rb.drag = 1f;
                        if (Time.time - DisengageStart < DisengageDuration)
                        {
                            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ImmediateGoal - transform.position, transform.up), Time.deltaTime *22f);
                            rb.AddRelativeForce(new Vector3(0, 0, 120f), ForceMode.Force);
                        }

                        else
                        {
                            Disengaging = false;
                        }
                    }

                    //This initiates the attack pattern and tracks its duration. Only check for this if "Disengaging" is set to "False"
                    else
                    {
                        if (!Attacking)
                        {
                            AttackStart = Time.time;
                            Attacking = true;
                        }

                        else
                        {
                            rb.drag = 5f;
                            if (Time.time - AttackStart < AttackDuration)
                            {
                                gameObject.SendMessage("FireMissile");
                            }

                            //At the end of attack phase, start the "Disengaging" phase:
                            else
                            {
                                if (!Disengaging)
                                {
                                    ImmediateGoal = Player.transform.position + transform.up * 100f;
                                    DisengageStart = Time.time;
                                    Disengaging = true;
                                }
                            }
                        }
                    }
                    
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
            //obstacle-avoidance, "Spotted" is not deactivated by angle alone. This should gives you a chance to escape the area.
            else
            {
                //When this ship is facing a new obstacle, lock on and plot a path towards the Player's last known position:
                if (front.collider == view.collider)
                {
                    if (Locked == false)
                    {
                        //PlotPath();
                        RePlotPath();
                    }

                    else
                    {

                        //If the locked path becomes obstructed, attempt to recalculate:
                        if (front.collider)
                        {
                            //First, check that the collier will actually be in the way

                            if (Vector3.Distance(transform.position, front.transform.position) < Vector3.Distance(transform.position, ImmediateGoal))
                            {
                                RePlotPath();
                            }

                            else
                            {

                            }

                        }

                        //If locked on and ship has not arrived at the first "node" of its path:
                        if (Vector3.Distance(transform.position, ImmediateGoal) >= 1f)
                        {
                            //Debug.Log(Vector3.Distance(transform.position, ImmediateGoal));
                            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ImmediateGoal - transform.position, transform.up), Time.deltaTime * 4f);
                            rb.AddRelativeForce(new Vector3(0, 0, 120f), ForceMode.Force);
                        }

                        else
                        {
                            //Debug.Log("Waypoint Reached");
                            if (ImmediateGoal != LastPosition)
                            {
                                ImmediateGoal = LastPosition;
                                //Debug.Log("Onward");
                            }
                            
                            else
                            {
                                if (front.collider)
                                {
                                    Locked = false;
                                }

                                if (Vector3.Distance(transform.position, ImmediateGoal) <= 1f)
                                {
                                    Locked = false;
                                }
                            }
                        }
                    }
                }

                //After fully turning away from the first obstale, or when player moves behind cover:
                else
                {
                    //This first case handles when the Player actively moves behind cover:
                    if (!Locked)
                    {
                        Locked = true;
                        ImmediateGoal = LastPosition;
                    }

                    if (Locked)
                    {
                        //If the path is Locked and a new obstacle is detected:
                        if (front.collider)
                        {
                            //First, check that the collier will actually be in the way
                            
                                if (Vector3.Distance(transform.position, front.transform.position) < Vector3.Distance(transform.position, ImmediateGoal))
                                {
                                    RePlotPath();
                                }

                                else
                                {

                                }
                            
                        }

                        //InvokeRepeating("NavRay", 0f, 0.1f);
                        if (Vector3.Distance(transform.position, ImmediateGoal) >= 1f)
                        {
                            //Debug.Log(Vector3.Distance(transform.position, ImmediateGoal));
                            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ImmediateGoal - transform.position, transform.up), Time.deltaTime * 4f);
                            rb.AddRelativeForce(new Vector3(0, 0, 120f), ForceMode.Force);
                        }

                        else
                        {
                            if (ImmediateGoal != LastPosition)
                            {
                                ImmediateGoal = LastPosition;
                                //Debug.Log("Onward");
                            }

                            else
                            {
                                if (Vector3.Distance(transform.position, ImmediateGoal) <= 1f)
                                {
                                    //The player is no longer at the last known position. Start a Search();
                                    if (!Searching)
                                    {
                                        Locked = false;
                                        SearchStart = Time.time;
                                        Searching = true;
                                    }

                                    else
                                    {
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }
    }

    void Seeking()
    {
        Physics.Raycast(transform.position, Player.transform.position - transform.position, out view, Range);
        Physics.Raycast(transform.position, transform.forward, out front, Range);
    }

    //When an obstacle comes between this ship and the Player, use this to avoid the first collision:
    void PlotPath()
    {
        Highroad = Vector3.Distance(transform.position, view.collider.transform.position + view.collider.bounds.size.y * transform.up * 1.3f);
        Highroad += Vector3.Distance(view.collider.transform.position + view.collider.bounds.size.y * transform.up * 1.3f, LastPosition);
        Lowroad = Vector3.Distance(transform.position, view.collider.transform.position + view.collider.bounds.size.y * -transform.up * 1.3f);
        Lowroad += Vector3.Distance(view.collider.transform.position + view.collider.bounds.size.y * -transform.up * 1.3f, LastPosition);
        if (Mathf.Min(Highroad, Lowroad) == Highroad)
        {
            ImmediateGoal = view.collider.transform.position + view.collider.bounds.size.y * transform.up * 1.3f;
        }

        else
        {
            ImmediateGoal = view.collider.transform.position + view.collider.bounds.size.y * -transform.up * 1.3f;
        }
        Locked = true;
    }

    //When the path is "locked" and a new obstacle is detected, use this to avoid the collision:
    void RePlotPath()
    {
        //Debug.Log(front.transform.gameObject);
        //Debug.Log("Replotting");
        float MaxDimension = Mathf.Max(front.collider.bounds.size.x, front.collider.bounds.size.y, front.collider.bounds.size.z);
        Highroad = Vector3.Distance(transform.position, front.collider.transform.position + MaxDimension * transform.up * 1.3f);
        Highroad += Vector3.Distance(front.collider.transform.position + MaxDimension * transform.up * 1.3f, LastPosition);
        Lowroad = Vector3.Distance(transform.position, front.collider.transform.position + MaxDimension * -transform.up * 1.3f);
        Lowroad += Vector3.Distance(front.collider.transform.position + MaxDimension * -transform.up * 1.3f, LastPosition);
        Leftroad = Vector3.Distance(transform.position, front.collider.transform.position + MaxDimension * -transform.right * 1.3f);
        Leftroad += Vector3.Distance(front.collider.transform.position + MaxDimension * -transform.right * 1.3f, LastPosition);
        Rightroad = Vector3.Distance(transform.position, front.collider.transform.position + MaxDimension * transform.right * 1.3f);
        Rightroad += Vector3.Distance(front.collider.transform.position + MaxDimension * transform.right * 1.3f, LastPosition);
        float ShortestPath = Mathf.Min(Highroad, Lowroad, Leftroad, Rightroad);
        if (ShortestPath == Highroad)
        {
            ImmediateGoal = front.collider.transform.position + MaxDimension * transform.up * 1.3f;
        }

        else if (ShortestPath == Lowroad)
        {
            ImmediateGoal = front.collider.transform.position + MaxDimension * -transform.up * 1.3f;
        }

        else if (ShortestPath == Leftroad)
        {
            ImmediateGoal = front.collider.transform.position + MaxDimension * -transform.right * 1.3f;
        }

        else
        {
            ImmediateGoal = front.collider.transform.position + MaxDimension * transform.right * 1.3f;
        }
        Locked = true;
    }

    public virtual void Patrol()
    {
        rb.AddRelativeForce(new Vector3(0, 0, 150f), ForceMode.Force);
        rb.rotation *= Quaternion.Euler(new Vector3(0, 0.4f, 0));
    }

    /*public virtual void Search()
    {
        Spotted = false;
        if (Time.time - SearchStart < SearchTime)
        {
            Searching = true;
            rb.rotation *= Quaternion.Euler(new Vector3(0, 0.4f, 0));
        }

        else
        {
            Searching = false;
        }
        
        //Debug.Log("Proceed to Search");
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
