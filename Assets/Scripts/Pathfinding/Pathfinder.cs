using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
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
    readonly int maxAccelerationCount = 100;
    int accelerationCount = 0;
    Vector3 nDirection = Vector3.zero;

    float turnRadius;
    float detectionRadius;

    Ship parent;
    GameObject refTarget;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        turnRadius = Mathf.Pow(300f, 2);
        detectionRadius = Mathf.Infinity;// turnRadius * 2f; ARBITRARY VALUES
    }

    private void Start()
    {
        refTarget = gameObject;
        parent = gameObject.GetComponent<Ship>();
        manager = gameObject.AddComponent<A>();
        manager.Init();

        if (parent == null)
        {
            Debug.Log("Unable to find Ship.cs on gameobject!");
            Destroy(this);
        }
    }

    public void SetTurnRadius(float newRadius)
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
        if ((refTarget == parent.GetMainTarget()))
            ExecuteBehaviour();
        else if ((parent.GetMainTarget() != null) && (Mathf.Pow(parent.GetMainTarget().transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(parent.GetMainTarget().transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(parent.GetMainTarget().transform.position.z - gameObject.transform.position.z, 2) <= detectionRadius)) // || (obstacle immediately in way))
        {
            for (int i = 0; i < followPath.Count; i++)
            {
                followPath[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            followPath.Clear();
            SetTarget(parent.GetMainTarget());
            recalculatePath = 0f;
        }
        else if (parent.GetPatrolPoints() != null)  // If target doesn't exist, follow patrol points
        {
            refTarget = null;
            SetGoals(parent.GetPatrolPoints());
        }
    }

    private void OrientShip()
    {
        nDirection = followPath[0].gameObject.transform.position - gameObject.transform.position;
        if (nDirection.normalized != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(nDirection.normalized) * angleOffset;
    }

    // Update is called once per frame
    private void ExecuteBehaviour()
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
                        // Visual effects. To be removed later
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
            else
                return;

            if ((isEngaged) && (Mathf.Pow(goal[goalCount].transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(goal[goalCount].transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(goal[goalCount].transform.position.z - gameObject.transform.position.z, 2) <= turnRadius))
                recalculatePath = 5f;   // Temporary cooldown. Replace this once turning is implemented
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

        // If the enemy is outside of pursuit radius, return false. Do not clear followPath in case this behaviour is meant to be the default and is still active
        if ((isEngaged) && ((goal[0] != null) && (Mathf.Pow(goal[0].transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(goal[0].transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(goal[0].transform.position.z - gameObject.transform.position.z, 2) <= pursuitRadius)))
            EndBehaviour();
    }

    private void EndBehaviour()
    {
        if (parent != null)
            SetGoals(parent.GetPatrolPoints());
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

    public void SetTarget(GameObject target)
    {
        refTarget = target;
        if (target != null)
        {
            isEngaged = true;
            SetGoal(target);
        }
        else if (parent != null)
            SetGoals(parent.GetPatrolPoints());
    }
}
