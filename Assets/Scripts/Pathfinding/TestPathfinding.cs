using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinding : MonoBehaviour
{
    List<GameObject> goal = new List<GameObject>();
    int goalCount = 0;
    List<Waypoint> followPath = new List<Waypoint>();
    float recalculatePath = 2f; // Recalculate every 2 seconds

    WaypointManager manager;    // Turn this static once things are figured out
    Quaternion angleOffset = Quaternion.Euler(0f, 0f, 0f); // Offset due to how the models are imported. Hardcoded

    float turnRadius = 30f;   // The player must be outside of this radius in order for the ship to turn properly
    Vector3 disengageDirection = Vector3.zero;    // If the target is within turnRadius when pathfinding is finished, then continue moving in last known direction
    bool isEngaged = false;
    float speed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        //goal = GameObject.FindWithTag("Player");

        manager = gameObject.GetComponent<WaypointManager>();
        manager.Init(); // TEMPORARY. MAKE WAYPOINT MANAGER STATIC AND REMOVE THIS LATER!!!

        if (goal.Count > 0)
        {
            followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform);

            if (followPath.Count > 0)
            {
                Vector3 direction = followPath[0].gameObject.transform.position - gameObject.transform.position;
                transform.rotation = Quaternion.LookRotation(direction.normalized) * angleOffset;
            }
        }
    }

    public void SetOffset(Quaternion offset)
    {
        angleOffset = offset;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * 
         * TEMPORARY CONDITION!!!!!!!!!!!!!!
         * 
         * 
         * */
        if (gameObject.GetComponent<Ship>().testPathfind)
        {
            recalculatePath -= Time.deltaTime;
            if (followPath.Count > 0)
            {
                if (followPath[0].transform.position == transform.position)
                {
                    followPath.RemoveAt(0);
                    if (recalculatePath <= 0)
                    {
                        recalculatePath = 2f;

                        // If the player is still able to be pursued (ie. outside of turn radius)
                        if ((!isEngaged) || ((isEngaged) && (Mathf.Pow(goal[goalCount].transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(goal[goalCount].transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(goal[goalCount].transform.position.z - gameObject.transform.position.z, 2) > turnRadius)))
                            followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform);
                        else
                            followPath.Clear(); // Otherwise clear the list (the player is too close for turning)
                    }

                    if (followPath.Count > 0)
                    {
                        Vector3 direction = followPath[0].gameObject.transform.position - gameObject.transform.position;
                        transform.rotation = Quaternion.LookRotation(direction.normalized) * angleOffset;
                    }
                }
                else
                    transform.position = Vector3.MoveTowards(transform.position, followPath[0].gameObject.transform.position, speed);
            }
            else
            {
                if (goal.Count > 0)
                {
                    goalCount = (goalCount + 1) % goal.Count;
                    recalculatePath = 0;
                }

                if ((isEngaged) && (Mathf.Pow(goal[goalCount].transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(goal[goalCount].transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(goal[goalCount].transform.position.z - gameObject.transform.position.z, 2) < turnRadius))
                {
                    gameObject.GetComponent<Rigidbody>().velocity = speed * 50 * gameObject.transform.forward;
                    recalculatePath = 3f;   // Temporary cooldown. Replace this once turning is implemented
                }
                else if (recalculatePath <= 0)
                {
                    followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform);
                    if (followPath.Count > 0)
                    {
                        Vector3 direction = followPath[0].gameObject.transform.position - gameObject.transform.position;
                        transform.rotation = Quaternion.LookRotation(direction.normalized) * angleOffset;
                    }
                    recalculatePath = 2f;
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }
    }

    public void SetEngage(bool engage)
    {
        isEngaged = engage;
    }
    public void SetTurnRadius(float radius)
    {
        turnRadius = Mathf.Pow(radius, 2);
    }
    public float GetTurnRadius()
    {
        return turnRadius;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void SetGoal(GameObject setGoal)
    {
        goal.Clear();
        goal.Add(setGoal);
        goalCount = 0;
    }
    public void AddGoal(GameObject addGoal)
    {
        goal.Add(addGoal);
    }
    public void SetGoals(GameObject[] setGoals)
    {
        goal.Clear();   // Clear all current waypoints
        goal.AddRange(setGoals); // Add in the new waypoints

        // Reset calculations
        goalCount = 0;
        if(goal.Count > 0)
            followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform);
        recalculatePath = 2f;
    }
}
