using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinding : MovementBehaviour
{
    List<GameObject> goal = new List<GameObject>();
    int goalCount = 0;
    List<Waypoint> followPath = new List<Waypoint>();
    float recalculatePath = 0f; // Recalculate every 2 seconds

    WaypointManager manager;    // Turn this static once things are figured out
    Quaternion angleOffset = Quaternion.Euler(0f, 0f, 0f); // Offset due to how the models are imported. Hardcoded

    Vector3 disengageDirection = Vector3.zero;    // If the target is within turnRadius when pathfinding is finished, then continue moving in last known direction
    bool isEngaged = false;
    float speed = 15f;
    float pursuitRadius;    // If the enemy is outside of this radius, the ship will no longer follow

    private void Awake()
    {
        priority = 1;
        manager = gameObject.GetComponent<WaypointManager>();
    }
    public override void SetTurnRadius(float newRadius)
    {
        turnRadius = Mathf.Pow(newRadius, 2); // TESTING
        detectionRadius = turnRadius * 2f;
        pursuitRadius = detectionRadius * 3f; // Arbitrary value. Enemies outside of this will not be pursued
    }
    public void SetOffset(Quaternion offset)
    {
        angleOffset = offset;
    }
    private void Update()
    {
        if ((parent?.GetMainTarget() != null) && (!activated) && Mathf.Pow(parent.GetMainTarget().transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(parent.GetMainTarget().transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(parent.GetMainTarget().transform.position.z - gameObject.transform.position.z, 2) <= detectionRadius) // || (obstacle immediately in way))
        {
            activated = true;
            followPath.Clear();
            SetTarget(parent.GetMainTarget());
            recalculatePath = 0f;
            PingParent();
        }
    }

    // Update is called once per frame
    public override bool ExecuteBehaviour()
    {
        recalculatePath -= Time.deltaTime;
        if (followPath.Count > 0)
        {
            if (gameObject.GetComponent<Rigidbody>() != null)
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

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
                    if(direction.normalized != Vector3.zero)
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
                if (goal.Count > 1)
                    recalculatePath = 0;
            }

            if ((isEngaged) && (Mathf.Pow(goal[goalCount].transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(goal[goalCount].transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(goal[goalCount].transform.position.z - gameObject.transform.position.z, 2) <= turnRadius))
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
                    if (direction.normalized != Vector3.zero)
                        transform.rotation = Quaternion.LookRotation(direction.normalized) * angleOffset;
                }
                recalculatePath = 2f;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        if (isEngaged)
        {
            // If the enemy is outside of pursuit radius, return false. Do not clear followPath in case this behaviour is meant to be the default and is still active
            return ((goal[0] != null) && (Mathf.Pow(goal[0].transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(goal[0].transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(goal[0].transform.position.z - gameObject.transform.position.z, 2) <= pursuitRadius));
        }
        return true;
       // return ((isEngaged) && (Mathf.Pow(goal[goalCount].transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(goal[goalCount].transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(goal[goalCount].transform.position.z - gameObject.transform.position.z, 2) <= pursuitRadius));
    }

    public override void EndBehaviour()
    {
        if (activated)
        {
            activated = false;
            totalPriority = priority;
            if (parent != null)
                SetGoals(parent.GetPatrolPoints());
        }
    }

    public void SetEngage(bool engage)
    {
        isEngaged = engage;
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
        isEngaged = false;
        goal.Clear();   // Clear all current waypoints
        goal.AddRange(setGoals); // Add in the new waypoints

        // Reset calculations
        goalCount = 0;
        if(goal.Count > 0)
            followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform);
        recalculatePath = 2f;
    }

    public override void SetTarget(GameObject target)
    {
        if (target != null)
        {
            totalPriority = priority + additionalPriority;
            isEngaged = true;
            SetGoal(target);
        }
        else if (parent != null)
        {
            totalPriority = priority;
            SetGoals(parent.GetPatrolPoints());
        }
    }
}
