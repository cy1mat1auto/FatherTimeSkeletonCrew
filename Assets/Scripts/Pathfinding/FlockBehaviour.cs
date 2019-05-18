using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Alternative to A*. Idea is to enable this and disable A* while the "lead" ship conducts actual pathfinding.
 * Using this is proposed to be less intensive than if all ships conducted A* at the same time despite runtime projected to be
 * O(n^2), n being the number of ships in game with this script attached. 
 * 
 * If an obstacle is within range, disengage and switch to A*
*/

// Must be attached to Ship.cs
public class FlockBehaviour : MovementBehaviour
{
    SphereCollider engageFlock;
    public List<Ship> flock = new List<Ship>();
   // public bool following = false;
    public Ship leader = null;
    float engageCooldown = 0f;

    private void Awake()
    {
        priority = 1.5f;
    }

    public override void SetTarget(GameObject target)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        engageFlock = gameObject.AddComponent<SphereCollider>();
        engageFlock.isTrigger = true;
        // Arbitrary setting of flock detection radius
        engageFlock.radius = gameObject.GetComponent<Ship>().GetTurnRadius() * 1.2f;
    }

    public override bool ExecuteBehaviour()
    {
        engageCooldown -= Time.deltaTime;

        // Refuse to execute behaviour if not following anyone, but do not EndBehaviour() unless explicitly told to by Ship.cs (ie. still activated)
        if ((leader == null) || (leader == gameObject.GetComponent<Ship>())) // || (distanceToShip is too far)
            return false;
    //    if (following)
        transform.position += (transform.forward + (Separation() + Orient()).normalized) * 15f;    // 15f is arbitrary speeds
        return true;
    }

    // What should happen after disengaging from behaviour
    public override void EndBehaviour()
    {
        if (activated)
        {
            if ((leader != null) && (leader.GetComponent<Ship>() != null))
                leader.RemoveFromFlock(this);

            //    following = false;
            activated = false;
            totalPriority = priority;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            // Use Physics.IgnoreLayer()
            if (other.tag == "Waypoint")
                return;
            // Take angle into account
            if ((other.GetComponent<Ship>() != null) && other.GetComponent<Ship>().GetName() == gameObject.GetComponent<Ship>().GetName())
            {
                // If other ship name matches own name and self is not following or being followed
                if ((engageCooldown <= 0))
                {
                    float distanceToShip = Mathf.Pow(gameObject.transform.position.x - other.gameObject.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - other.gameObject.transform.position.y, 2) + Mathf.Pow(gameObject.transform.position.z - other.gameObject.transform.position.z, 2);
                    if (gameObject.GetComponent<Ship>().GetTurnRadius() >= distanceToShip)  // If distance to ship is greater than own turning radius
                    {
                        bool autopilot = false; // Some conditions need to be met before FlockBehaviour activates

                        // If the other ship displays no flock behaviour, but can be followed (maybe add in a check of some sort)
                        if (other.gameObject.GetComponent<FlockBehaviour>() == null)
                            autopilot = ResignLeadership(other.gameObject.GetComponent<Ship>());
                        else if (other.gameObject.GetComponent<FlockBehaviour>().GetLeader() == null) // Otherwise if the other ship has no leader, make it the leader
                            autopilot = ResignLeadership(other.gameObject.GetComponent<Ship>());
                        else // Otherwise if the ship has or is a leader and within limit
                            autopilot = ResignLeadership(other.gameObject.GetComponent<FlockBehaviour>().GetLeader());
                        if (autopilot)
                        {
                            // following = true;
                            totalPriority = priority + additionalPriority;
                            PingParent();
                        }
                    }
                }
            }
        }
    }

    /*
     * FLOCKING BEHAVIOURS
     * 
     * - Separation: Avoid getting too close to neighbours
     * - Alignment: Orienting towards the average direction of the neighbours 
     * - Cohesion: Moving towards the average position of the neighbours
     * 
     */
    private Vector3 Separation()
    {
        /*
         * 
         * TEMPORARY!!!!!
         * 
         * */
        // If self were to be leader, would not actually engage in flock behaviour
        //if (leader != gameObject.GetComponent<Ship>())
        flock = leader.gameObject.GetComponent<Ship>().GetFlock();

        Vector3 position = Vector3.zero;

        for (int i = 0; i < flock.Count; i++)
        {
            // Add up all the distances between the object and flock members (already counted for as negative)
            if (flock[i] != gameObject.GetComponent<Ship>()) // Do not count self during calculations
                position += (transform.position - flock[i].transform.position);
        }

        position += transform.position - leader.gameObject.transform.position;

        position /= flock.Count; // Average the distances

        return position.normalized;
    }

    Vector3 Orient()
    {
        //if (leader != gameObject.GetComponent<Ship>())
        flock = leader.gameObject.GetComponent<Ship>().GetFlock();
        Vector3 position = Vector3.zero;  // Used for cohesion

        // There is always at least one (self) ship in the flock
        // Alignment and Cohesion put together
        for (int i = 0; i < flock.Count; i++)
        {
            position += flock[i].gameObject.transform.position; // Add up all the position vectors
        }

        position += leader.gameObject.transform.position;
        position /= (flock.Count + 1);    // Average the positions, including the leader

        transform.rotation = leader.gameObject.transform.rotation;    // Make the rotation the same as the leader

        return (position - transform.position).normalized; // Normalize the direction towards the average position (in this case the leader)
    }

    // Assign leader of the flock. If self is leader, pass control back to Ship.cs where it might revert to A* if it has it
    public void AssignLeader(Ship setLeader)
    {
        leader = setLeader;
    }

    public bool ResignLeadership(Ship newLeader)
    {
        // Transferring leadership of flock to a new ship newLeader. Return true if successful

        // If the ship is a leader and encounters a new leader that's not it's own with a large enough squad size
        if (/*(!following) && */(newLeader != gameObject.GetComponent<Ship>()) && (newLeader.GetFlockCount() + gameObject.GetComponent<Ship>().GetFlockCount() + 1 <= newLeader.GetMaxFlockCount()))
        {
            // Add all of the ship's current squad to the new leader
            for (int i = 0; i < gameObject.GetComponent<Ship>().GetFlockCount(); i++)
            {
                gameObject.GetComponent<Ship>().GetFlock()[i].gameObject.GetComponent<FlockBehaviour>().AssignLeader(newLeader);
                newLeader.AddToFlock(gameObject.GetComponent<Ship>().GetFlock()[i]);
            }
            newLeader.AddToFlock(gameObject.GetComponent<Ship>());  // Add self to the leader's squad
            gameObject.GetComponent<Ship>().ClearFlock();   // Clear own squad
            AssignLeader(newLeader);    // Assign the other ship as leader
            return true;
        }
        return false;
    }

    public Ship GetLeader()
    {
        return leader;
    }

    private void OnDestroy()
    {
        EndBehaviour();
    }
}
