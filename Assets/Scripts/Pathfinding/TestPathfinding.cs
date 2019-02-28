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
    Quaternion angleOffset = Quaternion.Euler(-90f, 0f, 0f); // Offset due to how the models are imported. Hardcoded

    // Start is called before the first frame update
    void Start()
    {
        //goal = GameObject.FindWithTag("Player");

        manager = gameObject.GetComponent<WaypointManager>();
        manager.Init(); // TEMPORARY. MAKE WAYPOINT MANAGER STATIC AND REMOVE THIS LATER!!!

        if(goal.Count > 0)
            followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform);
    }

    public void SetOffset(Quaternion offset)
    {
        angleOffset = offset;
    }

    // Update is called once per frame
    void Update()
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
                    followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, followPath[0].gameObject.transform.position, 15);
                Vector3 direction = followPath[0].gameObject.transform.position - gameObject.transform.position;
                transform.rotation = Quaternion.LookRotation(direction.normalized) * angleOffset;
            }
        }
        else
        {
            if (goal.Count > 1)
                goalCount = (goalCount + 1) % goal.Count;
            else if (goal.Count == 1)
            {
                Vector3 direction = goal[goalCount].transform.position - gameObject.transform.position;
                transform.rotation = Quaternion.LookRotation(direction.normalized) * angleOffset;
            }

            followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal[goalCount].transform);
            recalculatePath = 2f;
        }
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
