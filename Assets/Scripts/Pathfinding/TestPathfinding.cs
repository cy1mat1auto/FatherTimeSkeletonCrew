using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinding : MonoBehaviour
{
    GameObject goal;
    List<Waypoint> followPath = new List<Waypoint>();

    WaypointManager manager;    // Turn this static once things are figured out

    float distanceC = 0f;

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
        if (followPath.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, followPath[0].gameObject.transform.position, 10);
            distanceC += 10;

            if (followPath[0].transform.position == transform.position)
                followPath.RemoveAt(0);
        }
        else
            print(distanceC);
    }
}
