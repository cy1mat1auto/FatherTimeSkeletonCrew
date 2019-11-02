using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MovementBehaviour
{
    List<GameObject> goal = new List<GameObject>();
    int goalCount = 0;
    List<Waypoint> followPath = new List<Waypoint>();
    float recalculatePath = 0f; // Recalculate every 2 seconds

    A manager;    // Turn this static once things are figured out
    Quaternion angleOffset = Quaternion.Euler(0f, 0f, 0f); // Offset due to how the models are imported. Hardcoded

    bool isEngaged = false;
    float speed = 240f;
    float pursuitRadius;    // If the enemy is outside of this radius, the ship will no longer follow
    Rigidbody rb;
    readonly int maxAccelerationCount = 150;
    int accelerationCount = 0;
    Vector3 nDirection = Vector3.zero;

    private void Awake()
    {
        priority = 1;
        manager = gameObject.GetComponent<A>();
        manager.Init();
        rb = gameObject.GetComponent<Rigidbody>();
    }
    public override void SetTurnRadius(float newRadius)
    {
        turnRadius = Mathf.Pow(newRadius, 2); // TESTING
        detectionRadius = turnRadius * 2f;
        pursuitRadius = Mathf.Infinity;// detectionRadius * 3f; // Arbitrary value. Enemies outside of this will not be pursued
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
            for (int i = 0; i < followPath.Count; i++)
            {
                followPath[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            followPath.Clear();
            SetTarget(parent.GetMainTarget());
            recalculatePath = 0f;
            PingParent();
        }
    }

    private void OrientShip()
    {
        nDirection = followPath[0].gameObject.transform.position - gameObject.transform.position;
        if (nDirection.normalized != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(nDirection.normalized) * angleOffset;
    }

    // Update is called once per frame
    public override bool ExecuteBehaviour()
    {
        recalculatePath -= Time.deltaTime;
        if (followPath.Count > 0)
        {
            OrientShip();

            if (nDirection.magnitude < 250)
            {
                followPath[0].gameObject.GetComponent<MeshRenderer>().enabled = false;
                followPath.RemoveAt(0);

                if (recalculatePath <= 0)
                {
                    recalculatePath = 2f;
                    // If the player is still able to be pursued (ie. outside of turn radius)
                    if ((!isEngaged) || ((isEngaged) && (Mathf.Pow(goal[goalCount].transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(goal[goalCount].transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(goal[goalCount].transform.position.z - gameObject.transform.position.z, 2) > turnRadius)))
                        followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform, followPath);
                    else
                    {
                        // Visual effects
                        for (int i = 0; i < followPath.Count; i++)
                        {
                            followPath[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
                        }
                        followPath.Clear(); // Otherwise clear the list (the player is too close for turning)
                    }
                }

                if (followPath.Count > 0)
                {
                    rb.velocity = Vector3.zero;
                    OrientShip();
                    rb.AddForce(nDirection.normalized * speed * accelerationCount, ForceMode.Force);
                }
            }
            else if (accelerationCount < maxAccelerationCount)
            {
                accelerationCount++;
                rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
            }
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
                recalculatePath = 3f;   // Temporary cooldown. Replace this once turning is implemented
            else if (recalculatePath <= 0)
            {
                rb.velocity = Vector3.zero;
                followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform, followPath);
                if (followPath.Count > 0)
                {
                    OrientShip();
                    rb.AddForce(nDirection.normalized * accelerationCount * speed, ForceMode.Force);
                }
                recalculatePath = 2f;
            }
        }

        if (isEngaged)
        {
            // If the enemy is outside of pursuit radius, return false. Do not clear followPath in case this behaviour is meant to be the default and is still active
            return ((goal[0] != null) && (Mathf.Pow(goal[0].transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(goal[0].transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(goal[0].transform.position.z - gameObject.transform.position.z, 2) <= pursuitRadius));
        }
        return true;
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
        if (goal.Count > 0)
            followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform, followPath);
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
