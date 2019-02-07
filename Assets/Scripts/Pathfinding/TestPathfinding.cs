using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinding : MonoBehaviour
{
    GameObject goal;
    List<Waypoint> followPath = new List<Waypoint>();
    float recalculatePath = 2f; // Recalculate every 2 seconds

    WaypointManager manager;    // Turn this static once things are figured out

    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindWithTag("Finish");

        manager = gameObject.GetComponent<WaypointManager>();
        manager.Init(); // TEMPORARY. MAKE WAYPOINT MANAGER STATIC AND REMOVE THIS LATER!!!

        followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal.transform);
    }

    // Update is called once per frame
    void Update()
    {
        recalculatePath -= Time.deltaTime;

        if (recalculatePath <= 0)
        {
            recalculatePath = 2f;
            followPath = manager.FindPath(manager.FindClosestWaypoint(transform), goal.transform);
        }

        if (followPath.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, followPath[0].gameObject.transform.position, 10);
            transform.LookAt(followPath[0].gameObject.transform, Vector3.back);
            if (followPath[0].transform.position == transform.position)
                followPath.RemoveAt(0);
        }
    }
}
