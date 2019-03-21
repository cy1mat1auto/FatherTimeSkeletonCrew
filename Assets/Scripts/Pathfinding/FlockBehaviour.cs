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
    int maxFlockCount = 3;  // Maximum number of ships allowed within flock
    bool following = false;
    bool lead = false;  // If lead
    int leaderPresent = -1; // If found lead, this serves as an index for flock[] in identifying the lead

    private void OnTriggerEnter(Collider other)
    {
        // Take angle into account
        // Take flock count into ac-count
        if ((other.GetComponent<Ship>() != null) && (flock.Count == 0))
        {
            // If other ship name matches own name
            if (other.GetComponent<Ship>().GetName() == gameObject.GetComponent<Ship>().GetName())
            {
                float distanceToShip = Mathf.Pow(gameObject.transform.position.x - other.gameObject.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - other.gameObject.transform.position.y, 2) + Mathf.Pow(gameObject.transform.position.z - other.gameObject.transform.position.z, 2);
                if (gameObject.GetComponent<Ship>().GetTurnRadius() >= distanceToShip)  // If distance to ship is greater than own turning radius
                {
                    // If the other ship displays no flock behaviour, but can be followed (maybe add in a check of some sort)
                    if (other.gameObject.GetComponent<FlockBehaviour>() == null)
                    {
                        AssignLeader(flock.Count);  // last element before addition is flock.Count - 1. After addition, it becomes flock.Count
                    }
                    else if (other.gameObject.GetComponent<FlockBehaviour>().GetLeader() == null) // Otherwise if the other ship has no leader, make it the leader
                    {
                        AssignLeader(flock.Count);
                        other.GetComponent<FlockBehaviour>().AssignLeader(other.gameObject.GetComponent<FlockBehaviour>().AddToFlock(gameObject.GetComponent<Ship>()));
                    }
                    flock.Add(other.GetComponent<Ship>());
                    following = true;
                }
            }
        }
        gameObject.GetComponent<Ship>().testPathfind = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        engageFlock = gameObject.AddComponent<SphereCollider>();

        // Arbitrary setting of flock detection radius
        engageFlock.radius = gameObject.GetComponent<Ship>().GetTurnRadius() * 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            //  transform.position = Vector3.MoveTowards(transform.position, transform.position + (Separation() + Orient()) * 15, 15);
            transform.position += (Separation() + Orient()).normalized * 15f;
            //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
        Vector3 position = Vector3.zero;
        if (flock.Count > 0)
        {
            for (int i = 0; i < flock.Count; i++)
            {
                // Add up all the distances between the object and flock members (already counted for as negative)
                position += (flock[i].transform.position - transform.position);
            }

            position /= flock.Count; // Average the distances
        }
        return position.normalized;
    }

    Vector3 Orient()
    { 
        Vector3 position = transform.position;  // Used for cohesion
        if (flock.Count > 0)
        {
            // Alignment and Cohesion put together
            for (int i = 0; i < flock.Count; i++)
            {
                position += flock[i].gameObject.transform.position; // Add up all the position vectors
            }

            position /= flock.Count;    // Average the positions

            transform.rotation = flock[leaderPresent].gameObject.transform.rotation;    // Make the rotation the same as the leader
        }
        return (position - transform.position).normalized; // Normalize the direction towards the average position (in this case the leader)
    }

    // Assign leader of the flock. If self is leader, pass control back to Ship.cs where it might revert to A* if it has it
    public void AssignLeader(int setLeader)
    {
        if(setLeader <= -2)
            lead = true;
        else
        {
            lead = false;
            leaderPresent = setLeader;
        }
    }
    // Gets the leader of the flock
    public Ship GetLeader()
    {
        if (leaderPresent == -1)
            return null;
        else if (leaderPresent <= -2)
            return gameObject.GetComponent<Ship>();
        return flock[leaderPresent];
    }
    public void SetFlockCount(int setCount)
    {
        maxFlockCount = setCount;
    }
    public int GetMaxFlockCount()
    {
        return maxFlockCount;
    }
    public int GetFlockCount()
    {
        return flock.Count;
    }
    public int AddToFlock(Ship add)
    {
        if(flock.Count < maxFlockCount)
        {
            flock.Add(add);
            return flock.Count;
        }
        return -1;
    }
}
