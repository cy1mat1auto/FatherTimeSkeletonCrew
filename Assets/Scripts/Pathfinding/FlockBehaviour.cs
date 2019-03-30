using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Alternative to A*. Idea is to enable this and disable A* while the "lead" ship conducts actual pathfinding.
 * Using this is proposed to be less intensive than if all ships conducted A* at the same time, despite runtime projected to be
 * O(n^2), n being the number of ships in game with this script attached. 
 * 
 * If an obstacle is within range, disengage and switch to A*
*/

// Must be attached to Ship.cs
public class FlockBehaviour : MonoBehaviour
{
    SphereCollider engageFlock;
    List<Ship> flock = new List<Ship>();
    bool following = false;
    Ship leader = null;
    float engageCooldown = 0f;

    private void OnTriggerEnter(Collider other)
    {
        // Take angle into account
        // Take flock count into ac-count
        if ((other.GetComponent<Ship>() != null) && other.GetComponent<Ship>().GetName() == gameObject.GetComponent<Ship>().GetName())
        {
            // If other ship name matches own name and self is not following or being followed
            if ((!following) && (engageCooldown <= 0) && (leader == null))
            {
                float distanceToShip = Mathf.Pow(gameObject.transform.position.x - other.gameObject.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - other.gameObject.transform.position.y, 2) + Mathf.Pow(gameObject.transform.position.z - other.gameObject.transform.position.z, 2);
                if (gameObject.GetComponent<Ship>().GetTurnRadius() >= distanceToShip)  // If distance to ship is greater than own turning radius
                {
                    // If the other ship displays no flock behaviour, but can be followed (maybe add in a check of some sort)
                    if (other.gameObject.GetComponent<FlockBehaviour>() == null)
                    {
                        AssignLeader(other.gameObject.GetComponent<Ship>());  // last element before addition is flock.Count - 1. After addition, it becomes flock.Count
                        other.gameObject.GetComponent<Ship>().AddToFlock(gameObject.GetComponent<Ship>());
                    }
                    else if (other.gameObject.GetComponent<FlockBehaviour>().GetLeader() == null) // Otherwise if the other ship has no leader, make it the leader
                    {
                        AssignLeader(other.gameObject.GetComponent<Ship>());
                        other.GetComponent<FlockBehaviour>().AssignLeader(other.gameObject.GetComponent<Ship>());
                        other.gameObject.GetComponent<Ship>().AddToFlock(gameObject.GetComponent<Ship>());
                    }
                    else  // Otherwise if the ship has or is a leader
                    {
                        AssignLeader(other.gameObject.GetComponent<FlockBehaviour>().GetLeader());// AddToFlock(other.GetComponent<FlockBehaviour>().GetLeader()));

                        // Add self to leader's posse if they possess flock behaviour
                        if (leader.gameObject.GetComponent<FlockBehaviour>() != null)
                            leader.gameObject.GetComponent<Ship>().AddToFlock(gameObject.GetComponent<Ship>());

                    }
                    following = true;
                }
            }
            gameObject.GetComponent<Ship>().testPathfind = true;
        }
        else if (other.gameObject.tag != "Waypoint")
        {
            engageCooldown = 15f;
            gameObject.GetComponent<Ship>().testPathfind = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        engageFlock = gameObject.AddComponent<SphereCollider>();
        engageFlock.isTrigger = true;
        // Arbitrary setting of flock detection radius
        engageFlock.radius = gameObject.GetComponent<Ship>().GetTurnRadius() * 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        engageCooldown -= Time.deltaTime;
        if (following)
            transform.position += (transform.forward + (Separation() + Orient()).normalized) * 15f;
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
        if (leader != gameObject.GetComponent<Ship>())
            flock = leader.gameObject.GetComponent<Ship>().GetFlock();

        Vector3 position = Vector3.zero;
        if (flock.Count > 1)    // > 1 because it's assumed self will be a part of flock, but this should be ignored
        {
            for (int i = 0; i < flock.Count; i++)
            {
                // Add up all the distances between the object and flock members (already counted for as negative)
                if (flock[i] != gameObject.GetComponent<Ship>()) // Do not count self during calculations
                    position += (flock[i].transform.position - transform.position);
            }

            position += leader.gameObject.transform.position - transform.position;

            position /= flock.Count; // Average the distances
        }
        return position.normalized;
    }

    Vector3 Orient()
    {
        if (leader != gameObject.GetComponent<Ship>())
            flock = leader.gameObject.GetComponent<Ship>().GetFlock();
        Vector3 position = Vector3.zero;//transform.position;  // Used for cohesion
        if (flock.Count > 1)
        {
            // Alignment and Cohesion put together
            for (int i = 0; i < flock.Count; i++)
            {
                position += flock[i].gameObject.transform.position; // Add up all the position vectors
            }

            position += leader.gameObject.transform.position;
            position /= flock.Count;    // Average the positions

            transform.rotation = leader.gameObject.transform.rotation;    // Make the rotation the same as the leader
        }
        return (position - transform.position).normalized; // Normalize the direction towards the average position (in this case the leader)
    }

    // Assign leader of the flock. If self is leader, pass control back to Ship.cs where it might revert to A* if it has it
    public void AssignLeader(Ship setLeader)
    {
        leader = setLeader;
    }

    public Ship GetLeader()
    {
        return leader;
    }

    private void OnDestroy()
    {
        if ((following) && (leader != null) && (leader.GetComponent<Ship>() != null))
            leader.RemoveFromFlock(this);
    }
}
